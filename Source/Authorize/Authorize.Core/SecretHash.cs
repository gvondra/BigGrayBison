using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace BigGrayBison.Authorize.Core
{
    internal static class SecretHash
    {
        internal static byte[] Compute(string value, byte[] salt)
        {
            using Argon2i argon = new Argon2i(
                Encoding.UTF8.GetBytes(value))
            {
                DegreeOfParallelism = 8,
                MemorySize = 20480,
                Iterations = 32,
                Salt = salt
            };
            return argon.GetBytes(512);
        }

        internal static byte[] CreateSalt()
        {
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            byte[] salt = new byte[16];
            random.GetBytes(salt);
            return salt;
        }
    }
}
