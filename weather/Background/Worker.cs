using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using weather.Logic;
using weather.Logic.Database;
using System.Device.Gpio;
using System.Device.I2c;
using Iot.Device.CharacterLcd;
using Iot.Device.Pcx857x;
using Iot.Device.Common;
using Iot.Device.DHTxx;
using Microsoft.EntityFrameworkCore;

namespace weather.Background
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public IServiceProvider Services { get; }

        public Worker(ILogger<Worker> logger, IServiceProvider services)
        {
            _logger = logger;
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = Services.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<WeatherDBContext>();
                var _businessLogic = scope.ServiceProvider.GetRequiredService<BusinessLogic>();

                var migrations = await _db.Database.GetPendingMigrationsAsync();
                    if(migrations.Count() > 0)
                        await _db.Database.MigrateAsync();

                await _businessLogic.CreateConditionEntry(DateTime.UtcNow, 12, 24, 42);


                //Setup LCD Screen
                using I2cDevice i2c = I2cDevice.Create(new I2cConnectionSettings(1, 0x27));
                using var driver = new Pcf8574(i2c);
                using var lcd = new Lcd2004(registerSelectPin: 0, 
                                        enablePin: 2, 
                                        dataPins: new int[] { 4, 5, 6, 7 }, 
                                        backlightPin: 3, 
                                        backlightBrightness: 0.1f, 
                                        readWritePin: 1, 
                                        controller: new GpioController(PinNumberingScheme.Logical, driver));


                // GPIO Pin
                using (Dht11 dht = new Dht11(4))
                {
                    while(!stoppingToken.IsCancellationRequested)
                    {
                        var temperature = dht.Temperature;
                        var humidity = dht.Humidity;

                        if (dht.IsLastReadSuccessful && temperature.DegreesCelsius >= -20 && temperature.DegreesCelsius <= 60)
                        {
                            string temperatureString = $"{Math.Round(temperature.DegreesFahrenheit, 2)}F   {Math.Round(temperature.DegreesCelsius, 2)}C";
                            string humidityString = $"Humidity: {humidity.Percent}%";

                            await _businessLogic.CreateConditionEntry(DateTime.UtcNow, temperature.DegreesFahrenheit, temperature.DegreesCelsius, humidity.Percent);

                            lcd.Clear();

                            lcd.SetCursorPosition(0,0);
                            lcd.Write(temperatureString);

                            lcd.SetCursorPosition(0,1);
                            lcd.Write(humidityString);
                        }

                        //This is a slow sensor. You can only ready approx. every 2 seconds.
                        await Task.Delay(2000, stoppingToken);
                    }
                }
            }
        }
    }
}
