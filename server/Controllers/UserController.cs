using Microsoft.AspNetCore.Mvc;
using Nat20Server.Services;
using Nat20Server.Entities;
using Nat20Server.DTOs;
using Nat20Server.Mappings;

namespace Nat20Server.Controllers
{
    /// <summary>
    /// REST Controller for User management.
    /// Handles authentication, profile registration, and retrieval of user information.
    /// </summary>
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to retrieve.</param>
        /// <returns>The user if found, otherwise a 404 Not Found response.</returns>
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

        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <param name="request">The registration request containing user details.</param>
        /// <returns>A success message if registration is successful, otherwise an error message.</returns>
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
            return Ok("Registration successful.");
        }

        /// <summary>
        /// Authenticates a user with the provided login credentials.
        /// </summary>
        /// <param name="request">The login request containing user credentials.</param>
        /// <returns>The authentication response if successful, otherwise an error message.</returns>
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
