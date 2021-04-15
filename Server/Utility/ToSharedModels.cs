using System.Linq;

namespace daedalus.Server.Utility
{
    static public class ToSharedModels
    {
        static public Shared.Model.LoggedCondition ToSharedCondition(this Database.LoggedCondition model)
        {
            var condition = new Shared.Model.LoggedCondition()
            {
                Id = model.Id,
                LoggedAt = model.LoggedAt,
                DegreesCelsius = model.DegreesCelsius,
                AltitudeCentimeters = model.AltitudeCentimeters,
                HumidityPercentage = model.HumidityPercentage,
                PressureMillibars = model.PressureMillibars
            };

            return condition;
        }
    }
}
