using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Threading.Tasks;

namespace BigGrayBison.Common.Core
{
    public interface IKeyVault
    {
        Task<KeyVaultSecret> SetSecret(string vaultAddress, string name, string value);
        Task<KeyVaultSecret> GetSecret(string vaultAddress, string name);
        Task<JsonWebKey> CreateKey(string vaultAddress, string name, DateTime? expiration = null);
        Task<JsonWebKey> GetKey(string vaultAddress, string name, DateTime? expiration = null);
        Task<byte[]> Encrypt(string vaultAddress, string keyName, byte[] value, DateTime? expiration = null);
        Task<byte[]> Decrypt(string vaultAddress, string keyName, byte[] value);
    }
}
