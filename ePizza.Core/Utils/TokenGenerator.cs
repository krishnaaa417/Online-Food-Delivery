using ePizza.Models.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ePizza.Core.Utils
{
    public class TokenGenerator
    {
        private readonly IConfiguration _configuration;

        public TokenGenerator(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string GenerateToken(ValidateUserResponse userResponse)
        {
            string secretKey = _configuration["Jwt:Secret"]!;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor
                 = new SecurityTokenDescriptor
                 {
                     Subject = new ClaimsIdentity([

                         new Claim(ClaimTypes.Name,userResponse.Name),
                         new Claim(ClaimTypes.Email,userResponse.Email),
                         new Claim("UserId",userResponse.UserId.ToString()),
                         new Claim("IsAdmin",userResponse.Roles.Any(x => x.Equals("Admin")).ToString()),
                         new Claim("Roles", JsonSerializer.Serialize(userResponse.Roles))
                         ]),
                     Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:TokenExpiryInMinutes"])),
                     SigningCredentials = credentials,
                     Issuer = _configuration["Jwt:Issuer"],
                     Audience = _configuration["Jwt:Audience"]
                 };

            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
        }

        public ClaimsPrincipal? GetTokenPrincipal(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, // Ignore expiration for refresh scenario
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }


        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
