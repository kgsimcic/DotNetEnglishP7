using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dot.Net.WebApi.Services;

namespace Dot.Net.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BidController : Controller
    {
        private readonly IBidService _bidService;
        private readonly ILogger<BidController> _logger;

        public BidController(IBidService bidService, ILogger<BidController> logger)
        {
            _bidService = bidService;
            _logger = logger;
        }

        [HttpGet("/bids")]
        public IActionResult GetAllBids()
        {
            _logger.LogInformation("Connected to endpoint /bids!");
            var bids = _bidService.GetAllBids().ToList();
            return Ok(bids);
        }

        [HttpGet("/bids/{id}")]
        public ActionResult<Bid> GetBidById(int id)
        {
            _logger.LogInformation($"Connected to endpoint /bids/{id}!");

            var bid = _bidService.GetBid(id);

            if (bid == null)
            {
                return NotFound();
            }
            return Ok(bid);
        }

        public bool Validate([FromBody]Bid bid)
        {
            return ModelState.IsValid;
        }

        /*[HttpGet("/bid/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            return View("bidList/update");
        }*/

        [HttpPost("/bids")]
        public async Task<ActionResult> CreateBid([FromBody] Bid bid)
        {
            _logger.LogInformation("Connected to endpoint /bids!");

            if (bid == null)
            {
                return BadRequest("Bid cannot be null.");
            }

            var existingBid = _bidService.GetBid(bid.BidListId);
            if (existingBid != null)
            {
                return Conflict("A bid with this ID already exists.");
            }

            await _bidService.CreateBid(bid);

            return Created($"bid/{bid.BidListId}", bid);
        }

        [HttpPut("/bids/{id}")]
        public async Task<ActionResult> UpdateBid(int id, [FromBody] Bid bid)
        {
            _logger.LogInformation($"Connected to endpoint /bids/{id}!");

            if (bid == null) { return BadRequest("Bid cannot be null."); }

            if (id != bid.BidListId) { return BadRequest("ID in the URL does not match the ID of the bid."); }

            try
            {
                await _bidService.UpdateBid(id, bid);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("/bids/{id}")]
        public async Task<ActionResult> DeleteBid(int id)
        {
            _logger.LogInformation($"Connected to endpoint /bids/{id}!");

            try
            {
                await _bidService.DeleteBid(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}