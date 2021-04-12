using System;
using System.Collections.Generic;
using System.Text;

namespace weather.Logic.Model
{
    public class LoggedCondition
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime LoggedAt { get; set; }
        public double DegreesFahrenheit { get; set; }
        public double DegreesCelsius { get; set; }
        public double Humidity { get; set; }
    }
}
