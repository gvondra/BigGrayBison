using System.Collections.Generic;
using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Framework
{
    public interface ISigningKeyFactory
    {
        ISigningKey Create();
        Task<IEnumerable<ISigningKey>> GetAll(ISettings settings);
    }
}
