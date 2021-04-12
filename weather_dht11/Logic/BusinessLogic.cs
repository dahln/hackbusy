using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weather.Logic.Database;
using weather.Logic.Model;

namespace weather.Logic
{
    public class BusinessLogic
    {
        private readonly WeatherDBContext _db;
        public BusinessLogic(WeatherDBContext db)
        {
            _db = db;
        }

        async public Task CreateConditionEntry(DateTime loggedAt, double degreesFahrenheit, double degreesCelsius, double humidity)
        {
            LoggedCondition condition = new LoggedCondition()
            {
                DegreesFahrenheit = degreesFahrenheit,
                DegreesCelsius = degreesCelsius,
                Humidity = humidity,
                LoggedAt = loggedAt
            };

            _db.Conditions.Add(condition);
            await _db.SaveChangesAsync();
        }
    }
}
