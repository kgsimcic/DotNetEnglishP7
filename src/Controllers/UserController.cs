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
        private UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("/users")]
        public IActionResult Home()
        {
            var users = _userService.GetAllUsers().ToList();
            return Ok(users);
        }

        [HttpGet("/user/{userName}")]
        public async Task<ActionResult<User>> GetUserByUserName(string userName)
        {
            var user = await _userService.GetUserByName(userName);

            if (user == null) {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("/users")]
        public async Task<ActionResult<User>> CreateUser([FromBody]User user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null.");
            }

            // Check if a user with the same ID already exists
            var existingUser = _userService.GetUserById(user.Id);
            if (existingUser != null)
            {
                return Conflict("A user with this ID already exists.");
            }
            // Check if a user with the same username already exists
            existingUser = _userService.GetUserByName(user.UserName);
            if (existingUser != null)
            {
                return Conflict("A user with this username already exists.");
            }

            await _userService.AddUser(user);

            return Created($"user/{user.Id}", user);
        }

        public bool Validate([FromBody]User user)
        {
            return ModelState.IsValid;
        }
        
        //[HttpGet("/user/update/{id}")]
        //public IActionResult ShowUpdateForm(int id)
        //{
        //    User user = _userRepository.FindById(id);
            
        //    if (user == null)
        //        throw new ArgumentException("Invalid user Id:" + id);
            
        //    return View("user/update");
        //}

        [HttpPut("/user/update/{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (user == null) { return BadRequest("User cannot be null."); }

            if (id != user.Id) { return BadRequest("ID in the URL does not match the ID of the user."); }

            try
            {
                await _userService.UpdateUser(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();

        }

        [HttpDelete("/user/{id}")]
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