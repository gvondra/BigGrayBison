namespace BigGrayBison.Authorize.Data.Internal
{
    public class UserDataFactory : IUserDataFactory
    {
        private readonly IDbProviderFactory _providerFactory;
        private readonly IGenericDataFactory<UserData> _dataFactory;

        public UserDataFactory(IDbProviderFactory providerFactory, BrassLoon.DataClient.IGenericDataFactory<UserData> dataFactory)
        {
            _providerFactory = providerFactory;
            _dataFactory = dataFactory;
        }

        public async Task<UserData> GetByName(ISqlSettings settings, string name)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "name", DbType.String, DataUtil.GetParameterValue(name))
            };
            return (await _dataFactory.GetData(
                settings,
                _providerFactory,
                "[auth].[GetUserByName]",
                () => new UserData(),
                DataUtil.AssignDataStateManager,
                parameters))
                .FirstOrDefault();
        }
    }
}
