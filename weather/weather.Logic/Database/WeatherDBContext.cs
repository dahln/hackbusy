using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using weather.Logic.Model;

namespace weather.Logic.Database
{
    public class WeatherDBContext : DbContext
    {
        public WeatherDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<LoggedCondition> Series { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
