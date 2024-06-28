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
    public class CurvePointController : Controller
    {
        // TODO: Inject Curve Point service
        private readonly ICurvePointService _curvePointService;

        CurvePointController(ICurvePointService curvePointService)
        {
            _curvePointService = curvePointService;
        }

        [HttpGet("/curvepoints")]
        public IActionResult Home()
        {
            var curvePoints = _curvePointService.GetAllCurvePoints().ToList();
            return Ok(curvePoints);
        }

        [HttpGet("/curvepoints/{id}")]
        public async Task<ActionResult<CurvePoint>> GetCurvePointById(int id)
        {
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
            if (curvePoint == null)
            {
                return BadRequest("Curve point cannot be null.");
            }

            var existingCurvePoint = _curvePointService.GetCurvePoint(curvePoint.Id);
            if (existingCurvePoint != null)
            {
                return Conflict("A curve point with this ID already exists.");
            }

            await _curvePointService.CreateCurvePoint(curvePoint);
            return Created($"curvePoints/{curvePoint.Id}", curvePoint);
        }

        public bool Validate([FromBody]CurvePoint curvePoint)
        {
            return ModelState.IsValid;
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
            if (curvePoint == null) { return BadRequest("User cannot be null."); }

            if (id != curvePoint.Id) { return BadRequest("ID in the URL does not match the ID of the curve point."); }

            try
            {
                await _curvePointService.UpdateCurvePoint(id, curvePoint);
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