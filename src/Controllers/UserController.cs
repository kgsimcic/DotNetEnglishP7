using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Services;
using System.Text.Json;

namespace Dot.Net.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private UserRepository _userRepository;
        private UserService _userService;

        public UserController(UserService userService, UserRepository userRepository)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        [HttpGet("/user/list")]
        public IActionResult Home()
        {
            var users = _userRepository.FindAll().ToList();
            return Ok(users);
        }

        [HttpGet("/user/add")]
        public IActionResult AddUser([FromBody]User user)
        {
            
            return View("user/add");
        }

        [HttpGet("/user/validate")]
        public IActionResult Validate([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("user/add");
            }
           
           _userRepository.Add(user);
           
            return Redirect("user/list");
        }

        [HttpGet("/user/update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            User user = _userRepository.FindById(id);
            
            if (user == null)
                throw new ArgumentException("Invalid user Id:" + id);
            
            return View("user/update");
        }

        [HttpPost("/user/update/{id}")]
        public IActionResult updateUser(int id, [FromBody] User user)
        {
            // TODO: check required fields, if valid call service to update Trade and return Trade list

            return Redirect("/trade/list");
        }

        [HttpDelete("/user/{id}")]
        public IActionResult DeleteUser(int id)
        {
            User user = _userRepository.FindById(id);
            
            if (user == null)
                throw new ArgumentException("Invalid user Id:" + id);

            return Ok("User successfully deleted.");
        }
    }
}