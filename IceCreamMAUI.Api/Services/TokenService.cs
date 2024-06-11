using IceCreamMAUI.Shared.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IceCreamMAUI.Api.Services
{
    public class TokenService(IConfiguration configuration)
    {

        private readonly IConfiguration _configration = configuration;

        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration) =>
            new()
            {
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                IssuerSigningKey = GetSymmetricSecurityKey(configuration)
            };

        public string GenerateJwt(LoggedInUser user)
        {
            var securityKey = GetSymmetricSecurityKey(_configration);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var issuer = _configration["Jwt:Issuer"];
            var expireInMinutes = Convert.ToInt32(_configration["Jwt:ExpireInMinutes"]);
            Claim[] calims = [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.StreetAddress, user.Address)
                ];

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: "*",
                claims: calims,
                expires: DateTime.Now.AddMinutes(expireInMinutes),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }

        private static SymmetricSecurityKey GetSymmetricSecurityKey(IConfiguration configuration)
        {
            var secretKey = configuration["Jwt:SecretKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            return securityKey;
        }
    }
}
