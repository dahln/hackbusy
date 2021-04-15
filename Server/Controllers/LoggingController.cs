using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using daedalus.Shared;
using daedalus.Server.Database;
using Microsoft.EntityFrameworkCore;

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

        // [Route("api/v1/log/seed")]
        // [HttpGet]
        // async public Task<IActionResult> Seed() 
        // {
        //     for(int a = 0; a < 100; a++)
        //     {
        //         LoggedCondition loggedCondition = new LoggedCondition()
        //         {
        //             LoggedAt = DateTime.UtcNow,
        //             DegreesCelsius = 25,
        //             AltitudeCentimeters = 35,
        //             HumidityPercentage = 45,
        //             PressureMillibars = 55
        //         };
        //         _db.Conditions.Add(loggedCondition);
        //     }

        //     await _db.SaveChangesAsync();

        //     return Ok("Done");
        // }

        // [Route("api/v1/log/clear")]
        // [HttpGet]
        // async public Task<IActionResult> Clear() 
        // {
        //     var allConditions = _db.Conditions.Where(c => c.Id != null);
        //     _db.Conditions.RemoveRange(allConditions);
        //     await _db.SaveChangesAsync();

        //     return Ok("Cleared");
        // }

        [Route("api/v1/log/search")]
        [HttpGet]
        async public Task<IActionResult> SearchCondition() 
        {
            var results = await _db.Conditions.ToListAsync();

            return Ok(results);
        }
    }
}
