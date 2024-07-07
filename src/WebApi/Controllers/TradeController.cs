using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dot.Net.WebApi.Services;
using System.Diagnostics;

namespace Dot.Net.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TradeController : Controller
    {
        private readonly ITradeService _tradeService;

        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpGet("/trades")]
        public IActionResult GetAllTrades()
        {
            return Ok(_tradeService.GetAllTrades().ToList());
        }

        [HttpGet("/trades/{id}")]
        public async Task<ActionResult<Trade>> GetTradeById(int id)
        {
            var trade = await _tradeService.GetTrade(id);

            if (trade == null)
            {
                return NotFound();
            }
            return Ok(trade);
        }

        [HttpPost("/trades")]
        public async Task<ActionResult> CreateTrade([FromBody]Trade trade)
        {
            if (trade == null)
            {
                return BadRequest("Trade cannot be null.");
            }

            // Check if a user with the same ID already exists
            var existingTrade = _tradeService.GetTrade(trade.TradeId);
            if (existingTrade != null)
            {
                return Conflict("A trade with this ID already exists.");
            }

            await _tradeService.CreateTrade(trade);

            return Created($"trade/{trade.TradeId}", trade);
        }

        public bool Validate([FromBody]Trade trade)
        {
            return ModelState.IsValid;
        }

        /*[HttpGet("/trade/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get Trade by Id and to model then show to the form
            return View("trade/update");
        }*/

        [HttpPut("/trades/{id}")]
        public async Task<ActionResult> UpdateTrade(int id, [FromBody] Trade trade)
        {
            if (trade == null) { return BadRequest("Trade cannot be null."); }

            if (id != trade.TradeId) { return BadRequest("ID in the URL does not match the ID of the trade record."); }

            try
            {
                await _tradeService.UpdateTrade(id, trade);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("/trades/{id}")]
        public async Task<ActionResult> DeleteTrade(int id)
        {

            try
            {
                await _tradeService.DeleteTrade(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}