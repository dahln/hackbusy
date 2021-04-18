using System;
//using System.Device.I2c;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using Iot.Device.Bmxx80;
//using Iot.Device.Bmxx80.PowerMode;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace daedalus.iot
{
    class Program
    {
        private static IConfiguration configuration;
        async static Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configuration = builder.Build();

            var key = configuration["JWT_Key"];


            //var i2cSettings = new I2cConnectionSettings(1, Bme280.SecondaryI2cAddress);
            //using I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);
            //using var bme280 = new Bme280(i2cDevice);

            //int measurementTime = bme280.GetMeasurementDuration();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(configuration["Server"]);

            while (true)
            {
                //bme280.SetPowerMode(Bmx280PowerMode.Forced);
                //Thread.Sleep(measurementTime);

                //bme280.TryReadTemperature(out var tempValue);
                //bme280.TryReadPressure(out var preValue);
                //bme280.TryReadHumidity(out var humValue);
                //bme280.TryReadAltitude(out var altValue);

                var condition = new Shared.Model.Condition()
                {
                    LoggedAt = DateTime.UtcNow,
                    DegreesCelsius = 25,//tempValue.DegreesCelsius,
                    PressureMillibars = 25,//preValue.Millibars,
                    HumidityPercentage = 25,//humValue.Percent,
                    AltitudeCentimeters = 25//altValue.Centimeters
                };

                Console.WriteLine($"Date/Time: {condition.LoggedAt} UTC");
                Console.WriteLine($"Temperature: {condition.DegreesCelsius:0.#}\u00B0C");
                Console.WriteLine($"Pressure: {condition.PressureMillibars:#.##} millibars");
                Console.WriteLine($"Relative humidity: {condition.HumidityPercentage:#.##}%");
                Console.WriteLine($"Estimated altitude: {condition.AltitudeCentimeters:#} cm");

                //Prepare Condition JWT
                var jwt = JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(configuration["JWT_Key"])
                    .AddClaim("Condition", condition)
                    .MustVerifySignature()
                    .Encode();

                //Send Condition as JWT  to Server. Allows multiple 'conditions' but this is a single condition.
                Shared.Model.CondtionsAsJwt conditionsAsJWT = new Shared.Model.CondtionsAsJwt()
                {
                    JWT = jwt
                };
                var stringContent = new StringContent(JsonConvert.SerializeObject(conditionsAsJWT), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/v1/condition", stringContent);

                var message = await response.Content.ReadAsStringAsync();
                Console.WriteLine(message);

                //Thread.Sleep(1000); //This works, but it is more often than I need
                Thread.Sleep(60000); //New reading every 1 minute
            }
        }
    }
}

