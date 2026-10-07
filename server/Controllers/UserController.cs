using Microsoft.AspNetCore.Mvc;
using Nat20Server.Services;
using Nat20Server.Entities;
using Nat20Server.DTOs;
using Nat20Server.Mappings;

namespace Nat20Server.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUser = await _userService.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Conflict("A user with this email already exists.");
            }
            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email
            };
            await _userService.CreateUserAsync(newUser, request.Password);
            return CreatedAtAction(nameof(GetUserByEmail), new { email = newUser.Email }, newUser);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.GetUserByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }
            
            var userDto = user.ToPublicDto();
            return Ok(new AuthResponse { Token = "sample-jwt-token", User = userDto });
        }
    }
}
