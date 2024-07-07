using BigGrayBison.Authorize.Framework;
using System.Threading.Tasks;
using CommonCore = BigGrayBison.Common.Core;

namespace BigGrayBison.Authorize.Core
{
    public class AuthorizationCodeSaver : IAuthorizationCodeSaver
    {
        public Task Create(ISettings settings, IAuthorizationCode authorizationCode)
        {
            return CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), authorizationCode.Create);
        }

        public Task Update(ISettings settings, IAuthorizationCode authorizationCode)
        {
            return CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), authorizationCode.Update);
        }
    }
}
