using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Secrets;
using System.Threading.Tasks;

namespace BigGrayBison.Common.Core
{
    public interface IKeyVault
    {
        Task<KeyVaultSecret> SetSecret(string vaultAddress, string name, string value);
        Task<KeyVaultSecret> GetSecret(string vaultAddress, string name);
        Task<JsonWebKey> CreateKey(string vaultAddress, string name);
        Task<JsonWebKey> GetKey(string vaultAddress, string name);
    }
}
