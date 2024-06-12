using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
 
namespace Dot.Net.WebApi.Controllers
{
    [Route("[controller]")]
    public class BidListController : Controller
    {
        [HttpGet("/bidlist")]
        public IActionResult Home()
        {
            return View("Home");
        }

        [HttpGet("/bidList/validate")]
        public IActionResult Validate([FromBody]BidList bidList)
        {
            // TODO: check data valid and save to db, after saving return bid list
            return View("bidList/add");
        }

        [HttpGet("/bidList/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            return View("bidList/update");
        }

        [HttpPost("/bidList/update/{id}")]
        public IActionResult UpdateBid(int id, [FromBody] BidList bidList)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            return Redirect("/bidList/list");
        }

        [HttpDelete("/bidList/{id}")]
        public IActionResult DeleteBid(int id)
        {
            return Redirect("/bidList/list");
        }
    }
}