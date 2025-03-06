using System.Security.Cryptography;

namespace ASP.NET_store_project.Server.Utilities
{
    internal class SimplePasswordHasher(int saltSize = 32, int hashSize = 20, int iterations = 10000)
    {
        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, hashSize);

            byte[] hashBytes = new byte[saltSize + hashSize];
            Array.Copy(salt, 0, hashBytes, 0, saltSize);
            Array.Copy(hash, 0, hashBytes, saltSize, hashSize);

            return Convert.ToBase64String(hashBytes);
        }

        public void VerifyHash(string hashedPassword, string password)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[saltSize];
            Array.Copy(hashBytes, 0, salt, 0, saltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, hashSize);

            for (int i = 0; i < hashSize; i++)
                if (hashBytes[i + saltSize] != hash[i])
                    throw new UnauthorizedAccessException();
        }
    }
}