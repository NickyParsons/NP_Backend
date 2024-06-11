using System.Security.Cryptography;

namespace TestAspNetApplication.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 128 / 8;
        private const int KeySIze = 256 / 8;
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA256;
        private const char Delimiter = ';';

        public string GenerateHash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithm, KeySIze);
            return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public bool Verify(string storedHashedPassword, string inputPassword)
        {
            string[] elements = storedHashedPassword.Split(Delimiter);
            byte[] salt = Convert.FromBase64String(elements[0]);
            byte[] storedHash = Convert.FromBase64String(elements[1]);
            var inputHash = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, Iterations, _hashAlgorithm, KeySIze);
            return CryptographicOperations.FixedTimeEquals(inputHash, storedHash);
        }
    }
}
