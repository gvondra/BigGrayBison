using System.Collections.Generic;

namespace BigGrayBison.Authorize.Data
{
    public interface IAuthorizationCodeDataFactory
    {
        Task<IEnumerable<AuthorizationCodeData>> GetByClientIdIsActiveMinExpiration(ISqlSettings settings, Guid clientId, bool isActive, DateTime minExpiration);
    }
}
