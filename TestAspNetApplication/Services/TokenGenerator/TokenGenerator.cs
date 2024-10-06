using System.Security.Cryptography;

namespace TestAspNetApplication.Services
{
    public class TokenGenerator
    {
        public string GenerateToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
