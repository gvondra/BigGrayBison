namespace BigGrayBison.Authorize.Data
{
    public interface IUserDataSaver
    {
        Task Create(ITransactionHandler transactionHandler, UserData data, UserCredentialData userCredentialData);
        Task Update(ITransactionHandler transactionHandler, UserData data);
        Task SetUserCredential(ITransactionHandler transactionHandler, Guid userId, UserCredentialData data);
    }
}
