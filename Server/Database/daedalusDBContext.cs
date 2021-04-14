using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace daedalus.Server.Database
{
    public class daedalusDBContext : DbContext
    {
        public daedalusDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<LoggedCondition> Conditions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
