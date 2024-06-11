using TestAspNetApplication.Data.Entities;

namespace TestAspNetApplication.Services
{
    public interface IJwtProvider
    {
        public string GenerateToken(User user);
    }
}
