namespace BigGrayBison.Authorize.Data.Internal
{
    public class ClientDataFactory : IClientDataFactory
    {
        private readonly IDbProviderFactory _providerFactory;
        private readonly IGenericDataFactory<ClientData> _dataFactory;

        public ClientDataFactory(IDbProviderFactory providerFactory, IGenericDataFactory<ClientData> dataFactory)
        {
            _providerFactory = providerFactory;
            _dataFactory = dataFactory;
        }

        public async Task<ClientData> Get(ISqlSettings settings, Guid id)
        {
            IDataParameter[] parameters = new IDataParameter[]
            {
                DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid, id)
            };
            return (await _dataFactory.GetData(
                settings,
                _providerFactory,
                "[auth].[GetClient]",
                () => new ClientData(),
                DataUtil.AssignDataStateManager,
                parameters))
                .FirstOrDefault();
        }
    }
}
