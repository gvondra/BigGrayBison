using System.Collections.Generic;

namespace BigGrayBison.Authorize.Data.Internal
{
    public class SigningKeyDataFactory : ISigningKeyDataFactory
    {
        private readonly IDbProviderFactory _providerFactory;
        private readonly IGenericDataFactory<SigningKeyData> _dataFactory;

        public SigningKeyDataFactory(IDbProviderFactory providerFactory, IGenericDataFactory<SigningKeyData> dataFactory)
        {
            _providerFactory = providerFactory;
            _dataFactory = dataFactory;
        }

        public Task<IEnumerable<SigningKeyData>> GetAll(ISqlSettings settings)
        {
            return _dataFactory.GetData(
                settings,
                _providerFactory,
                "[auth].[GetSigningKey_All]",
                () => new SigningKeyData(),
                DataUtil.AssignDataStateManager,
                Enumerable.Empty<IDataParameter>());
        }
    }
}
