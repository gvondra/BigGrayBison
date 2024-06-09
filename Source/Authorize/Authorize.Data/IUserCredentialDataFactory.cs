using System.Collections.Generic;

namespace BigGrayBison.Authorize.Data
{
    public interface IUserCredentialDataFactory
    {
        Task<IEnumerable<UserCredentialData>> GetByUserId(ISqlSettings settings, Guid userId);
    }
}
