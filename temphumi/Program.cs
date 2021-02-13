using System;
using System.Device.I2c;
using System.Threading;
using Iot.Device.Common;
using Iot.Device.DHTxx;

namespace temphumi
{
    class Program
    {
        static void Main(string[] args)
        {
                // GPIO Pin
            using (Dht11 dht = new Dht11(4))
            {
                while(true)
                {
                    var temperature = dht.Temperature;
                    var humidity = dht.Humidity;

                    if (dht.IsLastReadSuccessful && temperature.DegreesCelsius >= -20 && temperature.DegreesCelsius <= 60)
                    {
                        Console.WriteLine($"Temperature: {Math.Round(dht.Temperature.DegreesFahrenheit, 2)} \u00B0F, Humidity: {dht.Humidity.Percent} %");
                    }

                    //This is a slow sensor. You can only ready approx. every 2 seconds.
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
