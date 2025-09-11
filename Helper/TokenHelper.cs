using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectD_API.Helper
{
    public class TokenHelper
    {
        public static string GenerateJwtToken(IConfiguration config, string userId, string username)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, username)
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                                issuer: config["Jwt:Issuer"],
                                audience: config["Jwt:Audience"],
                                claims: claims,
                                expires: DateTime.UtcNow.AddHours(int.Parse(config["Jwt:ExpirationHours"])),
                                signingCredentials: creds
                            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string? GetUserIdFromToken(IConfiguration config, string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]);
            try
            {
                var validations = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                var claims = handler.ValidateToken(token, validations, out var tokenSecure);
                return claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
