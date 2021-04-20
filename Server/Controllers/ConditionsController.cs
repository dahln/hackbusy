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
using Newtonsoft.Json;

namespace daedalus.Server.Controllers
{
    [ApiController]
    public class ConditionsController : ControllerBase
    {
        private readonly daedalusDBContext _db;
        private static IConfiguration _configuration;

        public ConditionsController(IConfiguration configuration, daedalusDBContext db)
        {
            _db = db;
            _configuration = configuration;
        }

        [Route("api/v1/condition")]
        [HttpPost]
        async public Task<IActionResult> LogCondition([FromBody] Shared.Model.CondtionsAsJwt model)
        {
            var conditions = model.Decode(_configuration["JWT_Key"]);
            if (conditions.Count == 0)
                return BadRequest("No Conditions Saved");

            var dbConditions = conditions.Select(c => c.ToCondition());

            _db.Conditions.AddRange(dbConditions);
            await _db.SaveChangesAsync();

            return Ok();
        }

        // [Route("api/v1/condition/clear")]
        // [HttpGet]
        // async public Task<IActionResult> Clear() 
        // {
        //     var allConditions = _db.Conditions.Where(c => c.Id != null);
        //     _db.Conditions.RemoveRange(allConditions);
        //     await _db.SaveChangesAsync();

        //     return Ok("Cleared");
        // }

        [Route("api/v1/condition/search/{start}/{end}")]
        [HttpGet]
        async public Task<IActionResult> SearchCondition(long start, long end) 
        {
            DateTime startFilter = new DateTime(start);
            DateTime endFilter = new DateTime(end);

            var results = await _db.Conditions.Where(c => c.LoggedAt >= startFilter && c.LoggedAt < endFilter).ToListAsync();

            List<Shared.Model.Condition> response = new List<Shared.Model.Condition>();

            if (results.Any())
                response = results.Select(r => r.ToSharedCondition()).OrderByDescending(r => r.LoggedAt).ToList();

            return Ok(response);
        }
    }
}
