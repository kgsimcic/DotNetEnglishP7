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
        private readonly ILogger<TradeController> _logger;

        public TradeController(ITradeService tradeService, ILogger<TradeController> logger)
        {
            _tradeService = tradeService;
            _logger = logger;
        }

        [HttpGet("/trades")]
        public async Task<ActionResult> GetAllTrades()
        {
            _logger.LogInformation("Connected to endpoint /trades!");
            return Ok(await _tradeService.GetAllTrades());
        }

        [HttpGet("/trades/{id}")]
        public async Task<ActionResult> GetTradeById(int id)
        {
            _logger.LogInformation($"Connected to endpoint /trades/{id}!");
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
            _logger.LogInformation("Connected to endpoint /trades!");
            if (trade == null)
            {
                return BadRequest("Trade cannot be null.");
            }

            // Check if a user with the same ID already exists
            var existingTrade = await _tradeService.GetTrade(trade.TradeId);
            if (existingTrade != null)
            {
                return Conflict("A trade with this ID already exists.");
            }

            var result = await _tradeService.CreateTrade(trade);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error.Description);
            }

            return Created($"trade/{trade.TradeId}", trade);
        }

        [HttpPut("/trades/{id}")]
        public async Task<ActionResult> UpdateTrade(int id, [FromBody] Trade trade)
        {
            _logger.LogInformation($"Connected to endpoint /trade/{id}!");

            if (trade == null) { return BadRequest("Trade cannot be null."); }

            if (id != trade.TradeId) { return BadRequest("ID in the URL does not match the ID of the trade record."); }

            try
            {
                var result = await _tradeService.UpdateTrade(id, trade);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Error.Description);
                }
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
            _logger.LogInformation($"Connected to endpoint /trades/{id}!");

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