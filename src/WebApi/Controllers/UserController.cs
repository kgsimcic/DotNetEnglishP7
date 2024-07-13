using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace Dot.Net.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
    }

        [HttpGet("/users")]
        public IActionResult Home()
        {
            _logger.LogInformation("Connected to endpoint /users!");
            var users = _userService.GetAllUsers().ToList();
            return Ok(users);
        }
        /*
                [HttpGet("/users/{userName}")]
                public async Task<ActionResult> GetUserByUserName(string userName)
                {
                    var user = await _userService.GetUserByName(userName);

                    if (user == null) {
                        return NotFound();
                    }
                    return Ok(user);
                }*/

        [HttpPost("/users")]
        public async Task<ActionResult> CreateUser([FromBody]User user)
        {
            _logger.LogInformation("Connected to endpoint /users!");

            if (user == null)
            {
                return BadRequest("User cannot be null.");
            }

            // Check if a user with the same ID already exists
            var existingUser = _userService.GetUserById(user.Id);
            if (existingUser != null)
            {
                Console.WriteLine("Id exists");
                return Conflict("A user with this ID already exists.");
            }
            // Check if a user with the same username already exists
            
            /*existingUser = await _userService.GetUserByName(user.UserName);
            if (existingUser != null)
            {
                Console.WriteLine("Username exists");
                return Conflict("A user with this username already exists.");
            }*/

            var result = await _userService.CreateUser(user);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error.Description);
            }

            return Created($"user/{user.Id}", user);
        }

        [Authorize]
        [HttpPut("/users/{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] User user)
        {
            _logger.LogInformation($"Connected to endpoint /users/{id}!");

            if (user == null) { return BadRequest("User cannot be null."); }

            if (id != user.Id) { return BadRequest("ID in the URL does not match the ID of the user."); }


            try
            {
                var result = await _userService.UpdateUser(id, user);
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

        [Authorize]
        [HttpDelete("/users/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            _logger.LogInformation($"Connected to endpoint /users/{id}!");

            try
            {
                await _userService.DeleteUser(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}