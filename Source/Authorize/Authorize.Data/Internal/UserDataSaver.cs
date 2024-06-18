using System.Data.Common;

namespace BigGrayBison.Authorize.Data.Internal
{
    public class UserDataSaver : IUserDataSaver
    {
        private readonly IDbProviderFactory _providerFactory;

        public UserDataSaver(IDbProviderFactory providerFactory)
        {
            _providerFactory = providerFactory;
        }

        public async Task Create(ITransactionHandler transactionHandler, UserData data, UserCredentialData userCredentialData)
        {
            transactionHandler.Connection ??= await _providerFactory.OpenConnection(transactionHandler);
            using (DbCommand command = transactionHandler.Connection.CreateCommand())
            {
                command.CommandText = "[auth].[CreateUser]";
                command.CommandType = CommandType.StoredProcedure;

                IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid);
                id.Direction = ParameterDirection.Output;
                _ = command.Parameters.Add(id);

                IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                timestamp.Direction = ParameterDirection.Output;
                _ = command.Parameters.Add(timestamp);

                command.Parameters.Add(
                    DataUtil.CreateParameter(_providerFactory, "name", DbType.String, DataUtil.GetParameterValue(data.Name)));
                command.Parameters.Add(
                    DataUtil.CreateParameter(_providerFactory, "emailAddressId", DbType.Guid, DataUtil.GetParameterValue(data.EmailAddressId)));
                command.Parameters.Add(
                    DataUtil.CreateParameter(_providerFactory, "isActive", DbType.Boolean, DataUtil.GetParameterValue(data.IsActive)));
                command.Parameters.Add(
                    DataUtil.CreateParameter(_providerFactory, "masterKey", DbType.Guid, DataUtil.GetParameterValue(userCredentialData.MasterKey)));
                command.Parameters.Add(
                    DataUtil.CreateParameter(_providerFactory, "secretSalt", DbType.Binary, DataUtil.GetParameterValue(userCredentialData.SecretSalt)));
                command.Parameters.Add(
                    DataUtil.CreateParameter(_providerFactory, "secretKey", DbType.Binary, DataUtil.GetParameterValue(userCredentialData.SecretKey)));
                command.Parameters.Add(
                    DataUtil.CreateParameter(_providerFactory, "secret", DbType.Binary, DataUtil.GetParameterValue(userCredentialData.Secret)));
                command.Parameters.Add(
                    DataUtil.CreateParameter(_providerFactory, "credentialExpiration", DbType.DateTime2, DataUtil.GetParameterValue(userCredentialData.Expiration)));

                _ = await command.ExecuteNonQueryAsync();
                data.UserId = (Guid)id.Value;
                data.CreateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
                data.UpdateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
            }
        }

        public async Task SetUserCredential(ITransactionHandler transactionHandler, Guid userId, UserCredentialData data)
        {
            await _providerFactory.EstablishTransaction(transactionHandler);
            using (DbCommand command = transactionHandler.Connection.CreateCommand())
            {
                command.CommandText = "[auth].[SetUserCredential]";
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = transactionHandler.Transaction.InnerTransaction;

                IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid);
                id.Direction = ParameterDirection.Output;
                _ = command.Parameters.Add(id);

                IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                timestamp.Direction = ParameterDirection.Output;
                _ = command.Parameters.Add(timestamp);

                command.Parameters.Add(
                    DataUtil.CreateParameter(_providerFactory, "userId", DbType.Guid, DataUtil.GetParameterValue(userId)));
                command.Parameters.Add(
                    DataUtil.CreateParameter(_providerFactory, "masterKey", DbType.Guid, DataUtil.GetParameterValue(data.MasterKey)));
                command.Parameters.Add(
                    DataUtil.CreateParameter(_providerFactory, "secretSalt", DbType.Binary, DataUtil.GetParameterValue(data.SecretSalt)));
                command.Parameters.Add(
                    DataUtil.CreateParameter(_providerFactory, "secretKey", DbType.Binary, DataUtil.GetParameterValue(data.SecretKey)));
                command.Parameters.Add(
                    DataUtil.CreateParameter(_providerFactory, "secret", DbType.Binary, DataUtil.GetParameterValue(data.Secret)));
                command.Parameters.Add(
                    DataUtil.CreateParameter(_providerFactory, "isActive", DbType.Boolean, DataUtil.GetParameterValue(data.IsActive)));
                command.Parameters.Add(
                    DataUtil.CreateParameter(_providerFactory, "expiration", DbType.DateTime2, DataUtil.GetParameterValue(data.Expiration)));

                _ = await command.ExecuteNonQueryAsync();
                data.UserCredentialId = (Guid)id.Value;
                data.CreateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
                data.UpdateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
                data.UserId = userId;
            }
        }
    }
}
