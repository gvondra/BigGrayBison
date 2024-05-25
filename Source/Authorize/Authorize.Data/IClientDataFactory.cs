namespace BigGrayBison.Authorize.Data
{
    public interface IClientDataFactory
    {
        Task<ClientData> Get(ISqlSettings settings, Guid id);
    }
}
