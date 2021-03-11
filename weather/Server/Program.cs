using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using weather.Background;
using weather.Logic;
using weather.Logic.Database;

namespace weather.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureServices(services =>
                {
                    //Setting up background worker
                    services.AddDbContext<WeatherDBContext>(options =>
                        options.UseSqlite($"Data Source=weather.db"));

                    services.AddScoped<BusinessLogic>();
                    services.AddHostedService<Worker>();
                });
    }
}
