using Azure;
using Azure.Security.KeyVault.Keys;
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

        public async Task<JsonWebKey> CreateKey(string vaultAddress, string name)
        {
            KeyClient client = new KeyClient(new Uri(vaultAddress), AzureCredential.DefaultAzureCredential);
            return (await CreateKey(client, name)).Key;
        }

        private async Task<KeyVaultKey> CreateKey(KeyClient client, string name)
        {
            Response<KeyVaultKey> response = await client.CreateRsaKeyAsync(
                new CreateRsaKeyOptions(name)
                {
                    Enabled = true,
                    KeySize = 2048
                });
            return response.Value;
        }

        public Task<JsonWebKey> GetKey(string vaultAddress, string name)
        {
            return _keyCache.ExecuteAsync(
                (context) =>
                {
                    KeyClient client = new KeyClient(new Uri(vaultAddress), AzureCredential.DefaultAzureCredential);
                    return GetKey(client, name);
                },
                new Context(name));
        }

        private async Task<JsonWebKey> GetKey(KeyClient client, string name)
        {
            JsonWebKey jsonWebKey = null;
            KeyVaultKey keyVaultKey = await GetKeyWithNotFoundCheck(client, name);
            if (keyVaultKey == null)
            {
                lock (_keyCache)
                {
                    keyVaultKey = GetKeyWithNotFoundCheck(client, name).Result;
                    if (keyVaultKey == null)
                    {
                        jsonWebKey = CreateKey(client, name).Result.Key;
                    }
                }
            }
            if (keyVaultKey != null)
            {
                jsonWebKey = keyVaultKey.Key;
            }
            return jsonWebKey;
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
