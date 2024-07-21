using BigGrayBison.Authorize.Framework;
using System.Threading.Tasks;
using CommonCore = BigGrayBison.Common.Core;

namespace BigGrayBison.Authorize.Core
{
    public class UserUpdater : IUserUpdater
    {
        public Task Update(ISettings settings, IUser user)
        {
            return CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), th => user.Update(settings, th));
        }
    }
}
