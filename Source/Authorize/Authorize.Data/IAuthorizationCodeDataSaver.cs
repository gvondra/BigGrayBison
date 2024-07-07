namespace BigGrayBison.Authorize.Data
{
    public interface IAuthorizationCodeDataSaver
    {
        Task Create(ITransactionHandler transactionHandler, AuthorizationCodeData data);
    }
}
