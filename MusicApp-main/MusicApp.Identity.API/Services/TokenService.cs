using Microsoft.IdentityModel.Tokens;
using MusicApp.Identity.API.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MusicApp.Identity.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly string secretKey;

        public TokenService(IConfiguration configuration)
        {
            secretKey = configuration["JwtBearer:SecretKey"]!;
        }

        public string GenerateLoginToken(ApplicationUser? user)
        {
            if (user is null)
                throw new ArgumentNullException();
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "User")
                ]);


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signInCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = signInCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var validationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha512 },
            };

            try
            {
                var principles = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principles;
            }
            catch (SecurityTokenExpiredException)
            {
                return null;
            }
        }
    }
}
