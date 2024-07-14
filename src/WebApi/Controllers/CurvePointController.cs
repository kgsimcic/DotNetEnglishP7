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
using System.Data;

namespace Dot.Net.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CurvePointController : Controller
    {
        private readonly ICurvePointService _curvePointService;
        private readonly ILogger<CurvePointController> _logger;

        CurvePointController(ICurvePointService curvePointService, ILogger<CurvePointController> logger)
        {
            _curvePointService = curvePointService;
            _logger = logger;
        }

        [HttpGet("/curvepoints")]
        public async Task<ActionResult> GetAllCurvePoints()
        {
            _logger.LogInformation("Connected to endpoint /curvepoints!");

            var curvePoints = await _curvePointService.GetAllCurvePoints();
            return Ok(curvePoints);
        }

        [HttpGet("/curvepoints/{id}")]
        public async Task<ActionResult> GetCurvePointById(int id)
        {
            _logger.LogInformation($"Connected to endpoint /curvepoints/{id}!");

            var curvePoint = await _curvePointService.GetCurvePoint(id);

            if (curvePoint == null)
            {
                return NotFound();
            }
            return Ok(curvePoint);
        }

        [HttpPost("/curvepoints")]
        public async Task<ActionResult> CreateCurvePoint([FromBody]CurvePoint curvePoint)
        {
            _logger.LogInformation("Connected to endpoint /curvepoints!");

            if (curvePoint == null)
            {
                return BadRequest("Curve point cannot be null.");
            }

            var existingCurvePoint = _curvePointService.GetCurvePoint(curvePoint.Id);
            if (existingCurvePoint != null)
            {
                return Conflict("A curve point with this ID already exists.");
            }

            var result = await _curvePointService.CreateCurvePoint(curvePoint);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error.Description);
            }

            return Created($"curvePoints/{curvePoint.Id}", curvePoint);
        }

        /*[HttpGet("/curvePoint/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            // TODO: get CurvePoint by Id and to model then show to the form
            return View("curvepoint/update");
        }*/

        [HttpPut("/curvepoints/{id}")]
        public async Task<ActionResult> UpdateCurvePoint(int id, [FromBody] CurvePoint curvePoint)
        {
            _logger.LogInformation($"Connected to endpoint /curvepoints/{id}!");

            if (curvePoint == null) { return BadRequest("User cannot be null."); }

            if (id != curvePoint.Id) { return BadRequest("ID in the URL does not match the ID of the curve point."); }

            try
            {
                var result = await _curvePointService.UpdateCurvePoint(id, curvePoint);
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

        [HttpDelete("/curvepoints/{id}")]
        public async Task<ActionResult> DeleteCurvePoint(int id)
        {
            _logger.LogInformation($"Connected to endpoint /curvepoints/{id}!");

            try
            {
                await _curvePointService.DeleteCurvePoint(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}