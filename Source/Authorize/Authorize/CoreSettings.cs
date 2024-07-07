using BrassLoon.Interface.Account;
using System;
using System.Threading.Tasks;

namespace Authorize
{
    public class CoreSettings : BigGrayBison.Authorize.Framework.ISettings
    {
        private readonly ITokenService _tokenService;
        private readonly Settings _settings;

        public CoreSettings(Settings settings, ITokenService tokenService)
        {
            _settings = settings;
            _tokenService = tokenService;
        }

        public bool UseDefaultAzureSqlToken => _settings.EnableDatabaseAccessToken;

        public string SigningKeyVaultAddress => _settings.SigningKeyVaultAddress;

        public Guid? AddressDomainId => _settings.AddressDomainId;

        public string EncryptionKeyVaultAddress => _settings.EncryptionKeyVaultAddress;

        public string TokenIssuer => _settings.TokenIssuer;

        string BrassLoon.Interface.Address.ISettings.BaseAddress => _settings.BrassLoonAddressApiBaseAddress;

        public Task<string> GetConnetionString() => Task.FromResult(_settings.ConnectionString);

        public Func<Task<string>> GetDatabaseAccessToken() => null;

        Task<string> BrassLoon.Interface.Address.ISettings.GetToken()
            => _tokenService.CreateClientCredentialToken(new AccountSettings(_settings), _settings.BrassLoonClientId.Value, _settings.BrassLoonClientSecret);
    }
}
