using Microsoft.AspNetCore.Mvc;
using ProjectD_API.Data;
using ProjectD_API.Email;
using ProjectD_API.Helper;
using ProjectD_API.Data.Models;
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
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        [GeneratedRegex(@"^[A-Za-z0-9._%+-]+@gmail\.com$")]
        private static partial Regex GmailRegex();


        public AuthController(GameDBContext context, IEmailService emailService, IConfiguration configuration)
        {
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Validate request data
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Username and Password are required");


            if (!GmailRegex().IsMatch(request.Email))
                return BadRequest("Email is not valid");

            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.UserName == request.Username))
                return Conflict("Username is already existed");

            // Create new user
            User user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.Username,
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
                return BadRequest("Username and Password are required");

            // Find user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username);
            if (user == null)
                return BadRequest("User not found");
            else if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return BadRequest("Incorrect password");

            var token = TokenHelper.GenerateJwtToken(_configuration, user.Id, user.UserName);

            return Ok(
                new LoginResponse()
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    Token = token,
                    RefreshToken = "",
                });
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators
        [HttpPost("validate-token")]
        public async Task<IActionResult> Verify([FromBody] string token)
        {
            var userId = TokenHelper.GetUserIdFromToken(_configuration, token);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Token is invalid or expired");

            return Ok(new { message = userId });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {

            // Validate request data
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required");

            if (!GmailRegex().IsMatch(email))
                return BadRequest("Email is not valid");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Check if email is exist
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null) return BadRequest("Email is not registered");

                var random = new Random();
                var pin = random.Next(1000, 9999);

                await _emailService.SendEmailAsync(email, "Password Reset PIN", $"Your password reset PIN is: {pin}");
                _context.PasswordResetRecords.Add(new PasswordResetRecord
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    PIN = pin,
                    ExpiryTime = DateTime.UtcNow.AddMinutes(15),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
                return Ok("PIN has been sent");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("validate-pin")]
        public async Task<IActionResult> ValidatePIN([FromBody] ValidatePINRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || (request.PIN == 0))
                return BadRequest("Email and PIN are required");
            if (request.PIN < 1000 || request.PIN > 10000)
                return BadRequest("PIN is not valid");

            var record = await _context.PasswordResetRecords.FirstOrDefaultAsync(r => r.Email == request.Email && r.PIN == request.PIN);
            if (record == null) return BadRequest("Change password request not found!");
            if (DateTime.UtcNow > record.ExpiryTime) return BadRequest("PIN is not valid anymore!");

            return Ok("PIN is valid");
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            /// TODO:
            /// Validate request data
            /// Find if user exist with email
            /// Serialize oldPassword and compare with database
            /// Update new password
            /// => That's all

            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.OldPassword) || string.IsNullOrEmpty(request.NewPassword))
                return BadRequest("Email, Old Password and New Password are required");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null) return BadRequest("Email is not registered");

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
                return BadRequest("Old Password is incorrect");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            _context.Users.Update(user);
            return Ok("Password has been changed successfully!");
        }
    }
}
