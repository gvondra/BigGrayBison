using System.Collections.Generic;

namespace BigGrayBison.Authorize.Data.Internal
{
    public class UserCredentialDataFactory : IUserCredentialDataFactory
    {
        private readonly IDbProviderFactory _providerFactory;
        private readonly IGenericDataFactory<UserCredentialData> _dataFactory;

        public UserCredentialDataFactory(IDbProviderFactory providerFactory, IGenericDataFactory<UserCredentialData> dataFactory)
        {
            _providerFactory = providerFactory;
            _dataFactory = dataFactory;
        }

        public Task<IEnumerable<UserCredentialData>> GetByUserId(ISqlSettings settings, Guid userId)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "userId", DbType.Guid, DataUtil.GetParameterValue(userId))
            };
            return _dataFactory.GetData(
                settings,
                _providerFactory,
                "[auth].[GetUserCredentialByUserId]",
                () => new UserCredentialData(),
                DataUtil.AssignDataStateManager,
                parameters);
        }
    }
}
