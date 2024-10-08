using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Dot.Net.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Net.Http.Headers;

namespace Dot.Net.WebApi.Controllers
{
    public class TokenResponse
    {
        public String Bearer { get; set; }
    }

    public class PartialUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }

    }

    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ITokenService tokenService, ILogger<UserController> logger)
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
            var partialUsers = users.Select(u => new PartialUser{
                Id = u.Id,
                UserName = u.UserName,
                FullName = u.FullName,
                Role = u.Role
                }).ToArray();

            return Ok(partialUsers);
        }

        [HttpGet("/users/{userName}")]
        public async Task<ActionResult> GetUserByUserName(string userName)
        {
            var user = await _userService.GetUserByName(userName);

            if (user == null)
            {
                return NotFound();
            }

            PartialUser partialUser = new PartialUser
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Role = user.Role
            };

            return Ok(partialUser);
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
                Bearer = token
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

            PartialUser partialUser = new PartialUser
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Role = user.Role
            };

            return Created($"user/{user.Id}", partialUser);
        }

        [HttpPut("/users/{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (user == null) { return BadRequest("User cannot be null."); }

            var existingUser = await _userService.GetUserById(id);

            if (existingUser == null)
            {
                return NotFound("No user with the ID in the URI was found.");
            }

            if (id != existingUser.Id) { return BadRequest("ID in the URI does not match the ID of the user."); }

            // test if token is valid/attached
            const string HeaderKeyName = "Bearer";

            StringValues headerValue = "";
            try
            {
                Request.Headers.TryGetValue(HeaderKeyName, out headerValue);
            }
            catch (Exception) { };

            int? validatedUser = _tokenService.ValidateToken(headerValue);
            if (validatedUser == null) {
                return Unauthorized("Please log in before editing user info.");
            }
            int userId = validatedUser.Value;

            // test that request parameters match user and claim matches user edited
            if (userId != user.Id)
            {
                return Unauthorized("You cannot edit user information that is not your own.");
            }

            try
            {
                var result = await _userService.UpdateUser(user);
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
            _logger.LogInformation($"Connected to endpoint /users/{id}!");

            // test if token is valid/attached
            const string HeaderKeyName = "Bearer";

            StringValues headerValue = "";
            try
            {
                Request.Headers.TryGetValue(HeaderKeyName, out headerValue);
            }
            catch (Exception) { };

            int? validatedUser = _tokenService.ValidateToken(headerValue);
            if (validatedUser == null)
            {
                return Unauthorized("Please log in before deleting user info.");
            }
            int userId = validatedUser.Value;

            // test that request parameters match user and claim matches user edited
            if (userId != id)
            {
                return Unauthorized("You cannot delete user information that is not your own.");
            }

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