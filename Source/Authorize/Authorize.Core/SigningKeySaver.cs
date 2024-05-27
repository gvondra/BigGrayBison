using BigGrayBison.Authorize.Framework;
using System.Threading.Tasks;
using CommonCore = BigGrayBison.Common.Core;

namespace BigGrayBison.Authorize.Core
{
    public class SigningKeySaver : ISigningKeySaver
    {
        public Task Create(ISettings settings, ISigningKey signingKey)
            => CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), th => signingKey.Create(th, settings));

        public Task Update(ISettings settings, ISigningKey signingKey)
            => CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), signingKey.Update);
    }
}
