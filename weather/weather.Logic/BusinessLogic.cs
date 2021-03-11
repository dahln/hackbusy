using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weather.Logic.Database;

namespace weather.Logic
{
    public class BusinessLogic
    {
        private readonly WeatherDBContext _db;
        public BusinessLogic(WeatherDBContext db)
        {
            _db = db;
        }

        public void CreateConditionEntry(DateTime loggedAt, double f)
        {
            return;
        }
    }
}
