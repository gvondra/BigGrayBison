using System;
using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Framework
{
    public interface IClientFactory
    {
        Task<IClient> Get(ISettings settings, Guid id);
    }
}
