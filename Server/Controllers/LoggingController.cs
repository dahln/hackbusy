using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using daedalus.Shared;
using daedalus.Server.Database;

namespace daedalus.Server.Controllers
{
    [ApiController]
    public class LoggingController : ControllerBase
    {
        private readonly daedalusDBContext _db;
        public LoggingController(daedalusDBContext db)
        {
            _db = db;
        }

        [Route("api/v1/log")]
        [HttpPost]
        async public Task<IActionResult> LogCondition([FromBody] Shared.Model.LoggedCondition model)
        {
            LoggedCondition loggedCondition = new LoggedCondition()
            {
                LoggedAt = model.LoggedAt,
                DegreesCelsius = model.DegreesCelsius,
                AltitudeCentimeters = model.AltitudeCentimeters,
                HumidityPercentage = model.HumidityPercentage,
                PressureMillibars = model.PressureMillibars
            };

            _db.Conditions.Add(loggedCondition);
            await _db.SaveChangesAsync();

            return Ok(loggedCondition.Id);
        }
    }
}
