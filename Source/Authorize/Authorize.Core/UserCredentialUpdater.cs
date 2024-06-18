using BigGrayBison.Authorize.Data;
using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using System;
using System.Threading.Tasks;
using CommonCore = BigGrayBison.Common.Core;

namespace BigGrayBison.Authorize.Core
{
    public class UserCredentialUpdater : IUserCredentialUpdater
    {
        private readonly IUserDataSaver _dataSaver;
        private readonly UserCredentialProcessor _userCredentialProcessor;

        public UserCredentialUpdater(
            IUserDataSaver dataSaver,
            CommonCore.IKeyVault keyVault)
        {
            _dataSaver = dataSaver;
            _userCredentialProcessor = new UserCredentialProcessor(keyVault);
        }

        public async Task Update(ISettings settings, Guid userId, string password)
        {
            UserCredentialData credentialData = await _userCredentialProcessor.Create(settings, password);
            await CommonCore.Saver.Save(
                new CommonCore.TransactionHandler(settings),
                th => _dataSaver.SetUserCredential(th, userId, credentialData));
        }
    }
}
