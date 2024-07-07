using BigGrayBison.Authorize.Data;
using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using BigGrayBison.Common.Core;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Core
{
    public class AuthorizationCode : IAuthorizationCode
    {
        private readonly AuthorizationCodeData _data;
        private readonly IAuthorizationCodeDataSaver _dataSaver;
        private readonly IKeyVault _keyVault;
        private readonly IUserFactory _userFactory;
        private readonly IClient _client;
        private IUser _user;

        public AuthorizationCode(
            AuthorizationCodeData data,
            IAuthorizationCodeDataSaver dataSaver,
            IKeyVault keyVault,
            IUserFactory userFactory,
            IUser user,
            IClient client)
        {
            _data = data;
            _dataSaver = dataSaver;
            _keyVault = keyVault;
            _userFactory = userFactory;
            _user = user;
            _client = client;
        }

        public AuthorizationCode(
            AuthorizationCodeData data,
            IAuthorizationCodeDataSaver dataSaver,
            IKeyVault keyVault,
            IUserFactory userFactory)
            : this(data, dataSaver, keyVault, userFactory, null, null)
        { }

        public Guid AuthorizationCodeId => _data.AuthorizationCodeId;

        private Guid UserId { get => _data.UserId; set => _data.UserId = value; }

        private Guid ClientId { get => _data.ClientId; set => _data.ClientId = value; }

        public Guid KeyId { get => _data.KeyId; internal init => _data.KeyId = value; }

        private byte[] Code { get => _data.Code; set => _data.Code = value; }

        public string State { get => _data.State; internal init => _data.State = value; }

        public DateTime Expiration { get => _data.Expiration; internal init => _data.Expiration = value; }

        public string RedirectUrl { get => _data.RedirectUrl; internal init => _data.RedirectUrl = value; }

        public bool IsActive { get => _data.IsActive; set => _data.IsActive = value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public DateTime UpdateTimestamp => _data.UpdateTimestamp;

        public Task Create(ITransactionHandler transactionHandler)
        {
            UserId = _user.UserId;
            ClientId = _client.ClientId;
            return _dataSaver.Create(transactionHandler, _data);
        }

        public Task Update(ITransactionHandler transactionHandler)
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> GetCode(Framework.ISettings settings)
        {
            return Code != null ? await _keyVault.Decrypt(settings.EncryptionKeyVaultAddress, KeyId.ToString("N"), Code) : null;
        }

        public async Task<byte[]> SetCode(Framework.ISettings settings)
        {
            byte[] code = RandomNumberGenerator.GetBytes(16);
            Code = await _keyVault.Encrypt(settings.EncryptionKeyVaultAddress, KeyId.ToString("N"), code, DateTime.UtcNow.AddMinutes(30));
            return code;
        }

        public async Task<IUser> GetUser(Framework.ISettings settings)
        {
            if (_user == null)
                _user = await _userFactory.Get(settings, UserId);
            return _user;
        }
    }
}
