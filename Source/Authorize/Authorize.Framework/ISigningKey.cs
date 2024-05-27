using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using CommonCore = BigGrayBison.Common.Core;

namespace BigGrayBison.Authorize.Framework
{
    public interface ISigningKey
    {
        Guid SigningKeyId { get; }
        bool IsActive { get; set; }
        DateTime CreateTimestamp { get; }
        DateTime UpdateTimestamp { get; }

        Task Create(CommonCore.ITransactionHandler transactionHandler, ISettings settings);
        Task Update(CommonCore.ITransactionHandler transactionHandler);

        Task<RsaSecurityKey> GetKey(ISettings settings, bool includePrivateKey = false);
    }
}
