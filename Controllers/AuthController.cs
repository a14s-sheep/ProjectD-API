using ProjectD_API.Data;
using ProjectD_API.Helper;
using ProjectD_API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using ProjectD_API.Data.Messages;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ProjectD_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public partial class AuthController : ControllerBase
    {
        private readonly GameDBContext _context;
        private readonly IConfiguration _configuration;

        [GeneratedRegex(@"^[A-Za-z0-9._%+-]+@gmail\.com$")]
        private static partial Regex GmailRegex();


        public AuthController(GameDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Validate request data
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return BadRequest(new { message = "Username and Password are required" });

            // TODO: Validate email with regex
            if (!GmailRegex().IsMatch(request.Email))
                return BadRequest(new { message = "Email is not valid" });

            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                return Conflict(new { message = "Username is already existed" });

            // Create new user
            User user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            };

            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();

            return Ok("Success");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Validate request data
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return BadRequest(new { message = "Username and Password are required" });

            // Find user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
                return BadRequest("User not found");
            else if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return BadRequest("Incorrect password");

            var token = TokenHelper.GenerateJwtToken(_configuration, user.Id, user.Username);

            return Ok(
                new LoginResponse()
                {
                    Token = token,
                    UserId = user.Id,
                    Username = user.Username
                });
        }


        [HttpPost("validate-token")]
        public async Task<IActionResult> Verify([FromBody] string token)
        {
            var userId = TokenHelper.GetUserIdFromToken(_configuration, token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest(new { message = "Token is invalid or expired" });

            return Ok(new { message = userId });
        }


    }
}
