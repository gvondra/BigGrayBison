using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.Caching.Memory;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CommonCore = BigGrayBison.Common.Core;

namespace BigGrayBison.Authorize.Core
{
    internal sealed class UserCredentialProcessor
    {
        private static readonly RSAEncryptionPadding _padding = RSAEncryptionPadding.OaepSHA512;
        private static readonly Policy _masterKeyCache = Policy.Cache(new MemoryCacheProvider(new MemoryCache(new MemoryCacheOptions())), TimeSpan.FromHours(4));
        private readonly CommonCore.IKeyVault _keyVault;

        public UserCredentialProcessor(CommonCore.IKeyVault keyVault)
        {
            _keyVault = keyVault;
        }

        internal async Task<UserCredentialData> Create(ISettings settings, string password)
        {
            byte[] salt = SecretHash.CreateSalt();
            byte[] hash = SecretHash.Compute(password, salt);

            using Aes aes = CreateAes();
            byte[] encrypted = Encrypt(aes, hash);
            byte[] ivAndKey = new byte[aes.IV.Length + aes.Key.Length];
            Array.Copy(aes.IV, 0, ivAndKey, 0, aes.IV.Length);
            Array.Copy(aes.Key, 0, ivAndKey, aes.IV.Length, aes.Key.Length);

            Guid masterKey = GetMasterKey();
            byte[] keyEncrypted = await _keyVault.Encrypt(settings.EncryptionKeyVaultAddress, masterKey.ToString("N"), ivAndKey);

            return new UserCredentialData
            {
                IsActive = true,
                MasterKey = masterKey,
                SecretSalt = salt,
                SecretKey = keyEncrypted,
                Secret = encrypted
            };
        }

        internal async Task<bool> IsAuthentic(ISettings settings, UserCredentialData userCredentialData, string password)
        {
            byte[] passwordHash = SecretHash.Compute(password, userCredentialData.SecretSalt);

            byte[] ivAndKey = await _keyVault.Decrypt(
                settings.EncryptionKeyVaultAddress,
                userCredentialData.MasterKey.Value.ToString("N"),
                userCredentialData.SecretKey);

            byte[] iv = new byte[16];
            byte[] aesKey = new byte[ivAndKey.Length - 16];
            Array.Copy(ivAndKey, 0, iv, 0, 16);
            Array.Copy(ivAndKey, 16, aesKey, 0, aesKey.Length);
            using Aes aes = CreateAes();
            aes.IV = iv;
            aes.Key = aesKey;
            byte[] hash = Decrypt(aes, userCredentialData.Secret);
            return hash.Length == passwordHash.Length && hash.SequenceEqual(passwordHash);
        }

        private static byte[] Encrypt(Aes aes, byte[] value)
        {
            using ICryptoTransform transform = aes.CreateEncryptor();
            using MemoryStream memoryStream = new MemoryStream();
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(value, 0, value.Length);
                cryptoStream.FlushFinalBlock();
            }
            return memoryStream.ToArray();
        }

        private static byte[] Decrypt(Aes aes, byte[] value)
        {
            using ICryptoTransform transform = aes.CreateDecryptor();
            using MemoryStream memoryStream = new MemoryStream();
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(value, 0, value.Length);
                cryptoStream.FlushFinalBlock();
            }
            return memoryStream.ToArray();
        }

        private static Aes CreateAes()
        {
            Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.GenerateKey();
            aes.GenerateIV();
            return aes;
        }

        private static Guid GetMasterKey()
        {
            return _masterKeyCache.Execute(
                context => Guid.NewGuid(),
                new Context());
        }
    }
}
