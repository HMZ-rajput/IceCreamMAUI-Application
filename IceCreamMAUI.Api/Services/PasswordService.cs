using System.Security.Cryptography;
using System.Text;

namespace IceCreamMAUI.Api.Services
{
    public class PasswordService
    {
        private const int saltSize = 10;

        public (string salt, string hashedPassword) GenerateSaltAndHash(string plainPassword)
        {
            if(string.IsNullOrWhiteSpace(plainPassword))
                throw new ArgumentNullException(nameof(plainPassword));

            var buffer = RandomNumberGenerator.GetBytes(saltSize);
            var salt = Convert.ToBase64String(buffer);

            var hashedPassword = GenerateHashedPassword(plainPassword, salt);

            return (salt,  hashedPassword);

        }

        public bool AreEqual(string plainPassword, string salt, string hashedPassword)
        {
            var newHashedPassword = GenerateHashedPassword(plainPassword, salt);

            return newHashedPassword == hashedPassword;
        }

        private static string GenerateHashedPassword(string plainPassword, string salt) 
        {
            var bytes = Encoding.UTF8.GetBytes(plainPassword + salt);
            var hash = SHA256.HashData(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}
