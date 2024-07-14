using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dot.Net.WebApi.Services;

namespace Dot.Net.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RuleController : Controller
    {
        private readonly IRuleService _ruleService;
        private readonly ILogger<RuleController> _logger;

        public RuleController(IRuleService ruleService, ILogger<RuleController> logger)
        {
            _ruleService = ruleService;
            _logger = logger;
        }

        [HttpGet("/rules")]
        public IActionResult GetAllRules()
        {
            _logger.LogInformation("Connected to endpoint /rules!");
            var result = _ruleService.GetAllRules().ToList();
            if (result is null)
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpGet("rules/{id}")]
        public ActionResult<Rule> GetRuleById(int id)
        {
            _logger.LogInformation($"Connected to endpoint /rules/{id}!");
            var rule = _ruleService.GetRule(id);
            if (rule == null)
            {
                return NotFound();
            }
            return Ok(rule);
        }

        [HttpPost("/rules")]
        public async Task<ActionResult> CreateRule([FromBody]Rule rule)
        {
            _logger.LogInformation("Connected to endpoint /rules!");

            if (rule == null)
            {
                return BadRequest("Rule cannot be null.");
            }

            var existingRule = _ruleService.GetRule(rule.Id);
            if (existingRule != null)
            {
                return Conflict("A rule with this ID already exists.");
            }

            await _ruleService.CreateRule(rule);
            return Created($"rule/{rule.Id}", rule);
        }

        /*[HttpGet("/ruleName/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get RuleName by Id and to model then show to the form
            return View("ruleName/update");
        }*/

        [HttpPut("/rules/{id}")]
        public async Task<ActionResult> UpdateRule(int id, [FromBody] Rule rule)
        {
            _logger.LogInformation($"Connected to endpoint /rules/{id}!");

            if (rule == null) { return BadRequest("Rule cannot be null."); }

            if (id != rule.Id) { return BadRequest("ID in the URL does not match the ID of the rule."); }

            try
            {
                await _ruleService.UpdateRule(id, rule);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("/rules/{id}")]
        public async Task<ActionResult> DeleteRule(int id)
        {
            _logger.LogInformation($"Connected to endpoint /rules/{id}!");

            try
            {
                await _ruleService.DeleteRule(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}