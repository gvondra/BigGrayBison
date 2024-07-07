using BigGrayBison.Common.Core;
using System;
using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Framework
{
    public interface IAuthorizationCode
    {
        Guid AuthorizationCodeId { get; }
        Guid KeyId { get; }
        string State { get; }
        DateTime Expiration { get; }
        string RedirectUrl { get; }
        //byte[] CodeChallenge { get; }
        //short CodeChallengeMethod { get; }
        bool IsActive { get; set; }
        DateTime CreateTimestamp { get; }
        DateTime UpdateTimestamp { get; }

        Task<byte[]> GetCode(ISettings settings);
        Task<byte[]> SetCode(ISettings settings);
        Task Create(ITransactionHandler transactionHandler);
        Task Update(ITransactionHandler transactionHandler);
        Task<IUser> GetUser(ISettings settings);
    }
}
