using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Framework
{
    public interface IAuthorizationCodeFactory
    {
        IAuthorizationCode Create(ISettings settings, IClient client, IUser user, string state, string redirectUrl);
        Task<IEnumerable<IAuthorizationCode>> GetByClientIdIsActiveMinExpiration(ISettings settings, Guid clientId, bool isActive, DateTime minExpiration);
        Task<IAuthorizationCode> GetActiveByCode(ISettings settings, Guid clientId, string code);
    }
}
