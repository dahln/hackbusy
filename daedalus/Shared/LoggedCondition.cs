using System;
using System.Collections.Generic;
using System.Text;

namespace daedalus.Shared.Model
{
    public class LoggedCondition
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime LoggedAt { get; set; }
        public double DegreesCelsius { get; set; }
        public double PressureMillibars { get; set; }
        public double HumidityPercentage { get; set; }
        public double AltitudeCentimeters { get; set; }
    }
}
