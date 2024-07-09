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

namespace Dot.Net.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("/users")]
        public IActionResult Home()
        {
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

        [HttpPut("/users/{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] User user)
        {
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

        [HttpDelete("/users/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
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