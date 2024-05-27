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
    }
}
