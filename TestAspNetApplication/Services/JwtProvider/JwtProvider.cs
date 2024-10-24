using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestAspNetApplication.Data.Entities;

namespace TestAspNetApplication.Services
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _config;
        public JwtProvider(IConfiguration configuration)
        { 
            _config = configuration;
        }
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("id", user.Id.ToString()));
            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email));
            claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role!.Name));
            string verified = (user.VerifiedAt != null ? true : false).ToString();
            claims.Add(new Claim("verified", verified));
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtOptions:SecretKey"]!)), 
                SecurityAlgorithms.HmacSha256
                );
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(double.Parse(_config["JwtOptions:ExpiresHours"]!))
                );
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }
}
