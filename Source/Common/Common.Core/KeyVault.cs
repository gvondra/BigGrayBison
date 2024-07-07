using Azure;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace BigGrayBison.Common.Core
{
    public class KeyVault : IKeyVault
    {
        private static readonly AsyncPolicy _secretCache = Policy.CacheAsync(new MemoryCacheProvider(new MemoryCache(new MemoryCacheOptions())), TimeSpan.FromMinutes(6));
        private static readonly AsyncPolicy _keyCache = Policy.CacheAsync(new MemoryCacheProvider(new MemoryCache(new MemoryCacheOptions())), TimeSpan.FromMinutes(6));

        public async Task<KeyVaultSecret> SetSecret(string vaultAddress, string name, string value)
        {
            SecretClient secretClient = new SecretClient(new Uri(vaultAddress), AzureCredential.DefaultAzureCredential);
            Azure.Response<KeyVaultSecret> kevaultSecret = await secretClient.SetSecretAsync(new KeyVaultSecret(name, value));
            return kevaultSecret.Value;
        }

        public Task<KeyVaultSecret> GetSecret(string vaultAddress, string name)
        {
            return _secretCache.ExecuteAsync(
                async context =>
                {
                    SecretClient secretClient = new SecretClient(new Uri(vaultAddress), AzureCredential.DefaultAzureCredential);
                    Azure.Response<KeyVaultSecret> kevaultSecret = await secretClient.GetSecretAsync(name);
                    return kevaultSecret.Value;
                },
                new Context(name));
        }

        public async Task<JsonWebKey> CreateKey(string vaultAddress, string name, DateTime? expiration = null)
        {
            KeyClient client = new KeyClient(new Uri(vaultAddress), AzureCredential.DefaultAzureCredential);
            return (await CreateKey(client, name, expiration)).Key;
        }

        private async Task<KeyVaultKey> CreateKey(KeyClient client, string name, DateTime? expiration)
        {
            CreateRsaKeyOptions options = new CreateRsaKeyOptions(name)
            {
                Enabled = true,
                KeySize = 2048
            };
            if (expiration.HasValue)
                options.ExpiresOn = expiration.Value;
            options.KeyOperations.Add(KeyOperation.Decrypt);
            options.KeyOperations.Add(KeyOperation.Encrypt);
            Response<KeyVaultKey> response = await client.CreateRsaKeyAsync(options);
            return response.Value;
        }

        public Task<JsonWebKey> GetKey(string vaultAddress, string name, DateTime? expiration = null)
        {
            return _keyCache.ExecuteAsync(
                async (context) =>
                {
                    KeyClient client = new KeyClient(new Uri(vaultAddress), AzureCredential.DefaultAzureCredential);
                    return (await GetKey(client, name, expiration))?.Key;
                },
                new Context(name));
        }

        public async Task<byte[]> Encrypt(string vaultAddress, string keyName, byte[] value, DateTime? expiration = null)
        {
            KeyClient client = new KeyClient(new Uri(vaultAddress), AzureCredential.DefaultAzureCredential);
            KeyVaultKey keyVaultKey = await GetKey(client, keyName, expiration);
            CryptographyClient cryptographyClient = client.GetCryptographyClient(keyVaultKey.Name, keyVaultKey.Properties.Version);
            EncryptResult result = await cryptographyClient.EncryptAsync(EncryptionAlgorithm.RsaOaep256, value);
            return result.Ciphertext;
        }

        public async Task<byte[]> Decrypt(string vaultAddress, string keyName, byte[] value)
        {
            KeyClient client = new KeyClient(new Uri(vaultAddress), AzureCredential.DefaultAzureCredential);
            KeyVaultKey keyVaultKey = await GetKeyWithNotFoundCheck(client, keyName);
            CryptographyClient cryptographyClient = client.GetCryptographyClient(keyVaultKey.Name, keyVaultKey.Properties.Version);
            DecryptResult result = await cryptographyClient.DecryptAsync(EncryptionAlgorithm.RsaOaep256, value);
            return result.Plaintext;
        }

        private async Task<KeyVaultKey> GetKey(KeyClient client, string name, DateTime? expiration = null)
        {
            KeyVaultKey keyVaultKey = await GetKeyWithNotFoundCheck(client, name);
            if (keyVaultKey == null)
            {
                lock (_keyCache)
                {
                    keyVaultKey = GetKeyWithNotFoundCheck(client, name).Result;
                    if (keyVaultKey == null)
                    {
                        keyVaultKey = CreateKey(client, name, expiration).Result;
                    }
                }
            }
            return keyVaultKey;
        }

        private async Task<KeyVaultKey> GetKeyWithNotFoundCheck(KeyClient client, string name)
        {
            try
            {
                Response<KeyVaultKey> response = await client.GetKeyAsync(name);
                return response.Value;
            }
            catch (RequestFailedException ex)
            {
                if (ex.Status == 404)
                    return null;
                else
                    throw;
            }
        }
    }
}
