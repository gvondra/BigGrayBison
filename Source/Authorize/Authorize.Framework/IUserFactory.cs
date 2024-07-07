using System;
using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Framework
{
    public interface IUserFactory
    {
        Task<IUser> Get(ISettings settings, Guid id);
        Task<IUser> GetByName(ISettings settings, string name);
        Task<bool> GetUserNameAvailable(ISettings settings, string name);
    }
}
