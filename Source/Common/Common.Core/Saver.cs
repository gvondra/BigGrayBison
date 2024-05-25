using System;
using System.Threading.Tasks;

namespace BigGrayBison.Common.Core
{
    public static class Saver
    {
        public static async Task Save(ITransactionHandler transactionHandler, Func<ITransactionHandler, Task> save)
        {
            try
            {
                await save(transactionHandler);
                if (transactionHandler.Transaction != null)
                    transactionHandler.Transaction.Commit();
                if (transactionHandler.Connection != null)
                    transactionHandler.Connection.Close();
            }
            catch
            {
                if (transactionHandler.Transaction != null)
                    transactionHandler.Transaction.Rollback();
                throw;
            }
            finally
            {
                if (transactionHandler.Transaction != null)
                {
                    transactionHandler.Transaction.Dispose();
                    transactionHandler.Transaction = null;
                }
                if (transactionHandler.Connection != null)
                {
                    transactionHandler.Connection.Dispose();
                    transactionHandler.Connection = null;
                }
            }
        }
    }
}
