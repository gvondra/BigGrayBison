using BigGrayBison.Authorize.Data;
using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using BigGrayBison.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Core
{
    public class SigningKeyFactory : ISigningKeyFactory
    {
        private readonly ISigningKeyDataFactory _dataFactory;
        private readonly ISigningKeyDataSaver _dataSaver;
        private readonly IKeyVault _keyVault;

        public SigningKeyFactory(ISigningKeyDataFactory dataFactory, ISigningKeyDataSaver dataSaver, IKeyVault keyVault)
        {
            _dataFactory = dataFactory;
            _dataSaver = dataSaver;
            _keyVault = keyVault;
        }

        public ISigningKey Create()
        {
            SigningKey result = Create(
                new SigningKeyData
                {
                    KeyVaultKey = Guid.NewGuid()
                });
            return result;
        }

        public async Task<ISigningKey> GetActive(Framework.ISettings settings)
        {
            return (await GetAll(settings))
                .OrderByDescending(k => k.CreateTimestamp)
                .FirstOrDefault();
        }

        public async Task<IEnumerable<ISigningKey>> GetAll(Framework.ISettings settings)
        {
            return (await _dataFactory.GetAll(new DataSettings(settings)))
                .Select<SigningKeyData, ISigningKey>(Create);
        }

        private SigningKey Create(SigningKeyData data) => new SigningKey(data, _dataSaver, _keyVault);
    }
}
