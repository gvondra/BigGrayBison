using System.Collections.Generic;

namespace BigGrayBison.Authorize.Data
{
    public interface ISigningKeyDataFactory
    {
        Task<IEnumerable<SigningKeyData>> GetAll(ISqlSettings settings);
    }
}
