using System.Collections;
using System.Data.Common;

namespace BigGrayBison.Authorize.Data.Internal
{
    public class AuthorizationCodeDataSaver : IAuthorizationCodeDataSaver
    {
        private readonly IDbProviderFactory _providerFactory;

        public AuthorizationCodeDataSaver(IDbProviderFactory providerFactory)
        {
            _providerFactory = providerFactory;
        }

        public async Task Create(ITransactionHandler transactionHandler, AuthorizationCodeData data)
        {
            if (data.Manager.GetState(data) == DataState.New)
            {
                await _providerFactory.EstablishTransaction(transactionHandler, data);
                using (DbCommand command = transactionHandler.Connection.CreateCommand())
                {
                    command.CommandText = "[auth].[CreateAuthorizationCode]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Transaction = transactionHandler.Transaction.InnerTransaction;

                    IDataParameter id = DataUtil.CreateParameter(_providerFactory, "id", DbType.Guid);
                    id.Direction = ParameterDirection.Output;
                    _ = command.Parameters.Add(id);

                    IDataParameter timestamp = DataUtil.CreateParameter(_providerFactory, "timestamp", DbType.DateTime2);
                    timestamp.Direction = ParameterDirection.Output;
                    _ = command.Parameters.Add(timestamp);

                    DataUtil.AddParameter(
                        _providerFactory, command.Parameters, "userId", DbType.Guid, DataUtil.GetParameterValue(data.UserId));
                    DataUtil.AddParameter(
                        _providerFactory, command.Parameters, "clientId", DbType.Guid, DataUtil.GetParameterValue(data.ClientId));
                    DataUtil.AddParameter(
                        _providerFactory, command.Parameters, "keyId", DbType.Guid, DataUtil.GetParameterValue(data.KeyId));
                    DataUtil.AddParameter(
                        _providerFactory, command.Parameters, "code", DbType.Binary, DataUtil.GetParameterValue(data.Code));
                    DataUtil.AddParameter(
                        _providerFactory, command.Parameters, "state", DbType.String, DataUtil.GetParameterValue(data.State));
                    DataUtil.AddParameter(
                        _providerFactory, command.Parameters, "expiration", DbType.DateTime2, DataUtil.GetParameterValue(data.Expiration));
                    DataUtil.AddParameter(
                        _providerFactory, command.Parameters, "redirectUrl", DbType.String, DataUtil.GetParameterValue(data.RedirectUrl));
                    DataUtil.AddParameter(
                        _providerFactory, command.Parameters, "codeChallenge", DbType.Binary, DataUtil.GetParameterValue(data.CodeChallenge));
                    DataUtil.AddParameter(
                        _providerFactory, command.Parameters, "codeChallengeMethod", DbType.Int16, DataUtil.GetParameterValue(data.CodeChallengeMethod));

                    AddCommonParameters(command.Parameters, data);

                    _ = await command.ExecuteNonQueryAsync();
                    data.AuthorizationCodeId = (Guid)id.Value;
                    data.CreateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
                    data.UpdateTimestamp = DateTime.SpecifyKind((DateTime)timestamp.Value, DateTimeKind.Utc);
                }
            }
        }

        private void AddCommonParameters(IList commandParameters, AuthorizationCodeData data)
        {
            commandParameters.Add(
                DataUtil.CreateParameter(_providerFactory, "isActive", DbType.Boolean, DataUtil.GetParameterValue(data.IsActive)));
        }
    }
}
