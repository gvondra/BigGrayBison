using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Framework
{
    public interface IUserAuthorizer
    {
        Task<IUser> Authorize(ISettings settings, string name, string password);
    }
}
