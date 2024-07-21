using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Framework
{
    public interface IUserUpdater
    {
        Task Update(ISettings settings, IUser user);
    }
}
