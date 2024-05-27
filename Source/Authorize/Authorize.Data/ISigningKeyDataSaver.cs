namespace BigGrayBison.Authorize.Data
{
    public interface ISigningKeyDataSaver
    {
        Task Create(ITransactionHandler transactionHandler, SigningKeyData data);
        Task Update(ITransactionHandler transactionHandler, SigningKeyData data);
    }
}
