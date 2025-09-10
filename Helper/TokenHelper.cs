using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectD_API.Helper
{
    public class TokenHelper
    {
        private const string SecretKey = "ThisIsA32ByteLongSuperSecretKey123!";
        public static string GenerateJwtToken(string userId, string username)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // ✅ Store UserId
            new Claim(ClaimTypes.Name, username)
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                                issuer: "MMOGameServer",
                                audience: "MMOClient",
                                claims: claims,
                                expires: DateTime.UtcNow.AddHours(3),
                                signingCredentials: creds
                            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static string GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(SecretKey);
            try
            {
                var validations = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "MMOGameServer",
                    ValidAudience = "MMOClient",
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                var claims = handler.ValidateToken(token, validations, out var tokenSecure);
                return claims.FindFirst(ClaimTypes.NameIdentifier)?.Value; // ✅ Extract UserId
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
