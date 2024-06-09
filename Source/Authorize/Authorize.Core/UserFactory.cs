using BigGrayBison.Authorize.Data;
using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using System.Threading.Tasks;

namespace BigGrayBison.Authorize.Core
{
    public class UserFactory : IUserFactory
    {
        private readonly IUserDataFactory _dataFactory;

        public UserFactory(IUserDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        public async Task<IUser> GetByName(ISettings settings, string name)
        {
            UserData userData = await _dataFactory.GetByName(new Common.Core.DataSettings(settings), name);
            return userData != null ? Create(userData) : null;
        }

        private static User Create(UserData data) => new User(data);
    }
}
