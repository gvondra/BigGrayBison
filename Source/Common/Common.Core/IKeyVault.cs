using Azure.Security.KeyVault.Secrets;
using System.Threading.Tasks;

namespace BigGrayBison.Common.Core
{
    public interface IKeyVault
    {
        Task<KeyVaultSecret> SetSecret(string vaultAddress, string name, string value);
        Task<KeyVaultSecret> GetSecret(string vaultAddress, string name);
    }
}
