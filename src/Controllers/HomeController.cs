using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
 
namespace Dot.Net.WebApi.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Home()
        {
            return View("Home");
        }

        [HttpGet("/Admin/Home")]
        public IActionResult Admin()
        {
            return View("/bidList/list");
        }
    }
}