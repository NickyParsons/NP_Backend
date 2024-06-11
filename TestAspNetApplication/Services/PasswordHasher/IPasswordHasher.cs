namespace TestAspNetApplication.Services
{
    public interface IPasswordHasher
    {
        string GenerateHash(string password);
        public bool Verify(string storedHashedPassword, string inputPassword);
    }
}
