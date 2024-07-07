using System.Collections.Generic;

namespace BigGrayBison.Authorize.Data.Internal
{
    public class AuthorizationCodeDataFactory : IAuthorizationCodeDataFactory
    {
        private readonly IDbProviderFactory _providerFactory;
        private readonly IGenericDataFactory<AuthorizationCodeData> _dataFactory;

        public AuthorizationCodeDataFactory(IDbProviderFactory providerFactory, IGenericDataFactory<AuthorizationCodeData> dataFactory)
        {
            _providerFactory = providerFactory;
            _dataFactory = dataFactory;
        }

        public Task<IEnumerable<AuthorizationCodeData>> GetByClientIdIsActiveMinExpiration(ISqlSettings settings, Guid clientId, bool isActive, DateTime minExpiration)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "clientId", DbType.Guid, DataUtil.GetParameterValue(clientId)),
                DataUtil.CreateParameter(_providerFactory, "isActive", DbType.Boolean, DataUtil.GetParameterValue(isActive)),
                DataUtil.CreateParameter(_providerFactory, "minExpiration", DbType.DateTime2, DataUtil.GetParameterValue(minExpiration))
            };
            return _dataFactory.GetData(
                settings,
                _providerFactory,
                "[auth].[GetAuthorizationCodeByClientIdIsActiveMinExpiration]",
                () => new AuthorizationCodeData(),
                DataUtil.AssignDataStateManager,
                parameters);
        }
    }
}
