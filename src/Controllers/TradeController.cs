using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Services;
using System.Diagnostics;

namespace Dot.Net.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TradeController : Controller
    {
        // TODO: Inject Trade service
        private readonly ITradeService _tradeService;

        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpGet("/trade/list")]
        public IActionResult GetAllTrades()
        {
            return Ok(_tradeService.GetAllTrades().ToList());
        }

        [HttpPost("/trade/add")]
        public async Task<ActionResult> AddTrade([FromBody]Trade trade)
        {
            if (!Validate(trade))
            {
                return BadRequest(ModelState);
            }

            var createdTrade = await _tradeService.AddTrade(trade);

            return Created($"/trade/{trade.TradeId}", createdTrade);
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

        [HttpPut("/trade/update/{id}")]
        public async Task<ActionResult> UpdateTrade(int id, [FromBody] Trade trade)
        {
            if (trade == null) { return BadRequest("Trade cannot be null."); }

            if (id != trade.TradeId) { return BadRequest("ID in the URL does not match the ID of the trade record."); }

            try
            {
                await _tradeService.UpdateTrade(trade);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("/trade/{id}")]
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