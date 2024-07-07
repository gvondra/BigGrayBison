using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Framework
{
    public interface IAuthorizationCodeSaver
    {
        Task Create(ISettings settings, IAuthorizationCode authorizationCode);
        Task Update(ISettings settings, IAuthorizationCode authorizationCode);
    }
}
