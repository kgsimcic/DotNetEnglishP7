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
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;

namespace Dot.Net.WebApi.Controllers
{
    public class TokenResponse
    {
        public String Token { get; set; }
    }

    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, TokenService tokenService, ILogger<UserController> logger)
        {
            _userService = userService;
            _tokenService = tokenService;
            _logger = logger;
    }

        [HttpGet("/users")]
        public async Task<ActionResult> GetAllUsers()
        {
            _logger.LogInformation("Connected to endpoint /users!");
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("/users/{userName}")]
        public async Task<ActionResult> GetUserByUserName(string userName)
        {
            var user = await _userService.GetUserByName(userName);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("/login")]
        public async Task<ActionResult> Login(User user)
        {
            _logger.LogInformation("Connected to endpoint /login!");
            string userName = user.UserName;
            string password = user.Password;

            var existingUser = await _userService.GetUserByName(userName);
            if (existingUser == null)
            {
                return BadRequest("User with this username does not exist.");
            }
            if (!_userService.CheckPassword(existingUser, password))
            {
                return BadRequest("Password incorrect.");
            }
            // generate token and return it to user
            string token = _tokenService.CreateToken(existingUser);
            var response = new TokenResponse
            {
                Token = token
            };
            return Ok(response);
        }

        [HttpPost("/register")]
        public async Task<ActionResult> Register([FromBody]User user)
        {
            _logger.LogInformation("Connected to endpoint /register!");

            if (user == null)
            {
                return BadRequest("New user cannot be null.");
            }

            // Check if a user with the same username already exists
            var existingUser2 = await _userService.GetUserByName(user.UserName);
            if (existingUser2 != null)
            {
                Console.WriteLine("Username exists");
                return Conflict("A user with this username already exists.");
            }

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
            var loggedInUser = this.User.Identity as ClaimsIdentity;
            _logger.LogInformation($"{loggedInUser.Name} connected to endpoint /users/{id}!");

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
            var loggedInUser = this.User.Identity as ClaimsIdentity;
            _logger.LogInformation($"{loggedInUser.Name} Connected to endpoint /users/{id}!");

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