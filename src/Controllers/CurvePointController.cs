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
    public class CurvePointController : Controller
    {
        // TODO: Inject Curve Point service
        private readonly ICurvePointService _curvePointService;

        CurvePointController(ICurvePointService curvePointService)
        {
            _curvePointService = curvePointService;
        }

        [HttpGet("/curvePoint/list")]
        public IActionResult Home()
        {
            var curvePoints = _curvePointService.GetAllCurvePoints().ToList();
            return Ok(curvePoints);
        }

        [HttpGet("/curvePoint/add")]
        public async Task<ActionResult> AddCurvePoint([FromBody]CurvePoint curvePoint)
        {

            return 1;
        }

        [HttpGet("/curvePoint/add")]
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

        [HttpPost("/curvepoint/update/{id}")]
        public IActionResult UpdateCurvePoint(int id, [FromBody] CurvePoint curvePoint)
        {
            if (curvePoint == null) { return BadRequest("User cannot be null."); }

            if (id != curvePoint.Id) { return BadRequest("ID in the URL does not match the ID of the curve point."); }

            try
            {
                await _curvePointService.UpdateCurvePoint(curvePoint);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("/curvepoint/{id}")]
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