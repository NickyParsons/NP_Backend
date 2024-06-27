using TestAspNetApplication.DTO;

namespace TestAspNetApplication.Services
{
    public interface IUserService
    {
        public Task Register(RegisterUserRequest form);
        public Task<string> Login(LoginUserRequest form);
    }
}
