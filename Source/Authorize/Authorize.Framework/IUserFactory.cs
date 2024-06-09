using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Framework
{
    public interface IUserFactory
    {
        Task<IUser> GetByName(ISettings settings, string name);
    }
}
