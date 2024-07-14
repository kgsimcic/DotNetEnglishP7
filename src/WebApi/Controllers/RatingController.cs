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
    public class RatingController : Controller
    {
        private readonly IRatingService _ratingService;
        private readonly ILogger<RatingController> _logger;

        public RatingController(IRatingService ratingService, ILogger<RatingController> logger)
        {
            _ratingService = ratingService;
            _logger = logger;
        }

        [HttpGet("/ratings")]
        public IActionResult GetAllRatings()
        {
            _logger.LogInformation("Connected to endpoint /ratings!");
            return Ok(_ratingService.GetAllRatings().ToList());
        }

        [HttpGet("/ratings/{id}")]
        public ActionResult<Rating> GetRatingById(int id)
        {
            _logger.LogInformation($"Connected to endpoint /ratings/{id}!");
            var rating = _ratingService.GetRating(id);

            if (rating == null)
            {
                return NotFound();
            }
            return Ok(rating);
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

        /*[HttpGet("/rating/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get Rating by Id and to model then show to the form
            return View("rating/update");
        }*/

        [HttpPut("/ratings/{id}")]
        public async Task<ActionResult> UpdateRating(int id, [FromBody] Rating rating)
        {
            _logger.LogInformation($"Connected to endpoint /ratings/{id}!");

            if (rating == null) { return BadRequest("Rating cannot be null."); }

            if (id != rating.Id) { return BadRequest("ID in the URL does not match the ID of the rating."); }

            try
            {
                await _ratingService.UpdateRating(id, rating);
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
            _logger.LogInformation($"Connected to endpoint /ratings/{id}!");

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