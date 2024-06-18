using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Framework
{
    public interface IUserCreator
    {
        Task<IUser> Create(ISettings settings, string userName, string password, string emailAddress);
    }
}
