using System;
using System.Device.Gpio;
using System.Device.I2c;
using System.Threading;
using Iot.Device.CharacterLcd;
using Iot.Device.Pcx857x;
using Iot.Device.Common;
using Iot.Device.DHTxx;

namespace temphumi
{
    class Program
    {
        static void Main(string[] args)
        {
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
                while(true)
                {
                    var temperature = dht.Temperature;
                    var humidity = dht.Humidity;

                    if (dht.IsLastReadSuccessful && temperature.DegreesCelsius >= -20 && temperature.DegreesCelsius <= 60)
                    {
                        string temperatureString = $"{Math.Round(dht.Temperature.DegreesFahrenheit, 2)}F   {Math.Round(dht.Temperature.DegreesCelsius, 2)}C";
                        string humidityString = $"Humidity: {dht.Humidity.Percent}%";

                        lcd.Clear();

                        lcd.SetCursorPosition(0,0);
                        lcd.Write(temperatureString);

                        lcd.SetCursorPosition(0,1);
                        lcd.Write(humidityString);
                    }

                    //This is a slow sensor. You can only ready approx. every 2 seconds.
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
