using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Services;

namespace Dot.Net.WebApi.Controllers
{
    [Route("[controller]")]
    public class RatingController : Controller
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet("/ratings")]
        public IActionResult Home()
        {
            return Ok(_ratingService.GetAllRatings().ToList());
        }

        [HttpPost("/ratings")]
        public async Task<ActionResult> CreateRating([FromBody]Rating rating)
        {
            if (rating == null)
            {
                return BadRequest("Rating cannot be null.");
            }

            var existingRating = _ratingService.GetRating(rating.Id);
            if (existingRating != null)
            {
                return Conflict("A rating with this ID already exists.");
            }

            await _ratingService.CreateRating(rating);
            return Created($"ratings/{rating.Id}", rating);
        }

        public bool Validate([FromBody]Rating rating)
        {
            return ModelState.IsValid;
        }

        /*[HttpGet("/rating/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get Rating by Id and to model then show to the form
            return View("rating/update");
        }*/

        [HttpPost("/rating/update/{id}")]
        public async Task<ActionResult> updateRating(int id, [FromBody] Rating rating)
        {
            if (rating == null) { return BadRequest("Rating cannot be null."); }

            if (id != rating.Id) { return BadRequest("ID in the URL does not match the ID of the rating."); }

            try
            {
                await _ratingService.UpdateRating(rating);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("/rating/{id}")]
        public async Task<ActionResult> DeleteRating(int id)
        {
            try
            {
                await _ratingService.DeleteRating(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}