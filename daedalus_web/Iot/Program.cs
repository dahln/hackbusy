using System;
using System.Device.I2c;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.PowerMode;
using Newtonsoft.Json;

namespace daedalus.iot
{
    class Program
    {
        async static Task Main(string[] args)
        {
            var i2cSettings = new I2cConnectionSettings(1, Bme280.SecondaryI2cAddress);
            using I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);
            using var bme280 = new Bme280(i2cDevice);

            int measurementTime = bme280.GetMeasurementDuration();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:5001/");

            while (true)
            {
                Console.Clear();

                bme280.SetPowerMode(Bmx280PowerMode.Forced);
                Thread.Sleep(measurementTime);

                bme280.TryReadTemperature(out var tempValue);
                bme280.TryReadPressure(out var preValue);
                bme280.TryReadHumidity(out var humValue);
                bme280.TryReadAltitude(out var altValue);

                var condition = new Shared.Model.LoggedCondition()
                {
                    LoggedAt = DateTime.UtcNow,
                    DegreesCelsius = tempValue.DegreesCelsius,
                    PressureMillibars = preValue.Millibars,
                    HumidityPercentage = humValue.Percent,
                    AltitudeCentimeters = altValue.Centimeters
                };

                //Console.WriteLine($"Date/Time: {condition.LoggedAt} UTC");
                //Console.WriteLine($"Temperature: {condition.DegreesCelsius:0.#}\u00B0C");
                //Console.WriteLine($"Pressure: {condition.PressureMillibars:#.##} millibars");
                //Console.WriteLine($"Relative humidity: {condition.HumidityPercentage:#.##}%");
                //Console.WriteLine($"Estimated altitude: {condition.AltitudeCentimeters:#} cm");

                var json = JsonConvert.SerializeObject(condition);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/v1/log", stringContent);

                await response.Content.ReadAsStringAsync();

                Thread.Sleep(1000);
            }
        }
    }
}

