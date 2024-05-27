using Azure.Security.KeyVault.Secrets;
using BigGrayBison.Authorize.Data;
using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using BrassLoon.JwtUtility;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CommonCore = BigGrayBison.Common.Core;

namespace BigGrayBison.Authorize.Core
{
    public class SigningKey : ISigningKey
    {
        private readonly SigningKeyData _data;
        private readonly ISigningKeyDataSaver _dataSaver;
        private readonly CommonCore.IKeyVault _keyVault;

        public SigningKey(
            SigningKeyData data,
            ISigningKeyDataSaver dataSaver,
            CommonCore.IKeyVault keyVault)
        {
            _data = data;
            _dataSaver = dataSaver;
            _keyVault = keyVault;
        }

        public Guid SigningKeyId => _data.SigningKeyId;

        public bool IsActive { get => _data.IsActive; set => _data.IsActive = value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public DateTime UpdateTimestamp => _data.UpdateTimestamp;

        internal Guid KeyVaultKey => _data.KeyVaultKey;

        public async Task Create(CommonCore.ITransactionHandler transactionHandler, ISettings settings)
        {
            await CreateKey(settings);
            await _dataSaver.Create(transactionHandler, _data);
        }

        public async Task<RsaSecurityKey> GetKey(Framework.ISettings settings, bool includePrivateKey = false)
        {
            KeyVaultSecret secret = await _keyVault.GetSecret(settings.SigningKeyVaultAddress, KeyVaultKey.ToString("D"));
            return RsaSecurityKeySerializer.GetSecurityKey(secret.Value, includePrivateKey);
        }

        public Task Update(CommonCore.ITransactionHandler transactionHandler)
        {
            return _dataSaver.Update(transactionHandler, _data);
        }

        private async Task CreateKey(Framework.ISettings settings)
        {
            using (RSA serviceProvider = RSA.Create(2048))
            {
                RSAParameters rsaParameters = serviceProvider.ExportParameters(true);
                _ = await _keyVault.SetSecret(settings.SigningKeyVaultAddress, KeyVaultKey.ToString("D"), RsaSecurityKeySerializer.Serialize(rsaParameters));
            }
        }
    }
}
