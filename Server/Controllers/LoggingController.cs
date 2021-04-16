using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using daedalus.Shared;
using daedalus.Server.Database;
using Microsoft.EntityFrameworkCore;
using daedalus.Server.Utility;
using JWT.Builder;
using JWT.Algorithms;
using Microsoft.Extensions.Configuration;

namespace daedalus.Server.Controllers
{
    [ApiController]
    public class LoggingController : ControllerBase
    {
        private readonly daedalusDBContext _db;
        private static IConfiguration _configuration;

        public LoggingController(IConfiguration configuration, daedalusDBContext db)
        {
            _db = db;
            _configuration = configuration;
        }

        [Route("api/v1/log")]
        [HttpPost]
        async public Task<IActionResult> LogCondition([FromBody] Shared.Model.JwtEncodedCondition model)
        {
            var loggedConditionFromJwt = JwtBuilder.Create()
                   .WithAlgorithm(new HMACSHA256Algorithm())
                   .WithSecret(_configuration["JWT_Key"])
                   .MustVerifySignature()
                   .Decode<Shared.Model.LoggedCondition>(model.Content);

            if (loggedConditionFromJwt == null)
                return BadRequest();

            LoggedCondition loggedCondition = new LoggedCondition()
            {
                LoggedAt = loggedConditionFromJwt.LoggedAt,
                DegreesCelsius = loggedConditionFromJwt.DegreesCelsius,
                AltitudeCentimeters = loggedConditionFromJwt.AltitudeCentimeters,
                HumidityPercentage = loggedConditionFromJwt.HumidityPercentage,
                PressureMillibars = loggedConditionFromJwt.PressureMillibars
            };

            _db.Conditions.Add(loggedCondition);
            await _db.SaveChangesAsync();

            return Ok(loggedCondition.Id);
        }

        [Route("api/v1/log/seed")]
        [HttpGet]
        async public Task<IActionResult> Seed() 
        {
            for(int a = 0; a < 100; a++)
            {
                LoggedCondition loggedCondition = new LoggedCondition()
                {
                    LoggedAt = DateTime.UtcNow,
                    DegreesCelsius = 25,
                    AltitudeCentimeters = 35,
                    HumidityPercentage = 45,
                    PressureMillibars = 55
                };
                _db.Conditions.Add(loggedCondition);
            }

            await _db.SaveChangesAsync();

            return Ok("Done");
        }

        [Route("api/v1/log/clear")]
        [HttpGet]
        async public Task<IActionResult> Clear() 
        {
            var allConditions = _db.Conditions.Where(c => c.Id != null);
            _db.Conditions.RemoveRange(allConditions);
            await _db.SaveChangesAsync();

            return Ok("Cleared");
        }

        [Route("api/v1/log/search/{start}/{end}")]
        [HttpGet]
        async public Task<IActionResult> SearchCondition(long start, long end) 
        {
            DateTime startFilter = new DateTime(start);
            DateTime endFilter = new DateTime(end);

            var results = await _db.Conditions.Where(c => c.LoggedAt >= startFilter && c.LoggedAt < endFilter).ToListAsync();

            List<Shared.Model.LoggedCondition> response = new List<Shared.Model.LoggedCondition>();

            if (results.Any())
                response = results.Select(r => r.ToSharedCondition()).OrderByDescending(r => r.LoggedAt).ToList();

            return Ok(response);
        }
    }
}
