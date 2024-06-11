namespace TestAspNetApplication.Services
{
    public interface IUserService
    {
        public Task Register(string email, string password, string? firstname, string? lastname);
        public Task<string> Login(string username, string password);
    }
}
