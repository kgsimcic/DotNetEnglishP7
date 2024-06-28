using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Services;

namespace Dot.Net.WebApi.Controllers
{
    [Route("[controller]")]
    public class BidController : Controller
    {
        private readonly IBidService _bidService;

        [HttpGet("/bids")]
        public IActionResult Home()
        {
            var bids = _bidService.GetAllBids().ToList();
            return Ok(bids);
        }

        [HttpGet("/bid/validate")]
        public bool Validate([FromBody]Bid bidList)
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
        [HttpPut("/bid/update/{id}")]
        public async Task<ActionResult> UpdateBid(int id, [FromBody] Bid bid)
        {
            if (bid == null) { return BadRequest("Bid cannot be null."); }

            if (id != bid.BidListId) { return BadRequest("ID in the URL does not match the ID of the bid."); }

            try
            {
                await _bidService.UpdateBid(bid);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("/bid/{id}")]
        public async Task<ActionResult> DeleteBid(int id)
        {
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