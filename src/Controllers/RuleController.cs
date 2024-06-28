using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Services;

namespace Dot.Net.WebApi.Controllers
{
    [Route("[controller]")]
    public class RuleController : Controller
    {
        // TODO: Inject RuleName service
        private readonly IRuleService _ruleService;

        public RuleController(IRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        [HttpGet("/ruleName/list")]
        public IActionResult Home()
        {
            // TODO: find all RuleName, add to model
            var result = _ruleService.GetAllRules().ToList();
            if (result is null)
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpPost("/ruleName/add")]
        public async Task<ActionResult> AddRuleName([FromBody]Rule rule)
        {
            if (!Validate(rule))
            {
                return BadRequest(ModelState.IsValid);
            }
            await _ruleService.CreateRule(rule);

            return Created();
        }

        public bool Validate([FromBody]Rule ruleName)
        {
            return ModelState.IsValid;
        }

        /*[HttpGet("/ruleName/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get RuleName by Id and to model then show to the form
            return View("ruleName/update");
        }*/

        [HttpPut("/ruleName/update/{id}")]
        public async Task<ActionResult> updateRule(int id, [FromBody] Rule rule)
        {
            if (rule == null) { return BadRequest("Rule cannot be null."); }

            if (id != rule.Id) { return BadRequest("ID in the URL does not match the ID of the rule."); }

            try
            {
                await _ruleService.UpdateRule(rule);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("/rule/{id}")]
        public async Task<ActionResult> DeleteRule(int id)
        {
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