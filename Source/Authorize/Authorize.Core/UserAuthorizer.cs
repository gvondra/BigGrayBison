using BigGrayBison.Authorize.Data;
using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonCore = BigGrayBison.Common.Core;

namespace BigGrayBison.Authorize.Core
{
    public class UserAuthorizer : IUserAuthorizer
    {
        private const double _minimumResponseSeconds = 1.5;
        private readonly IUserFactory _userFactory;
        private readonly IUserCredentialDataFactory _userCredentialDataFactory;
        private readonly UserCredentialProcessor _userCredentialProcessor;

        public UserAuthorizer(IUserFactory userFactory, IUserCredentialDataFactory userCredentialDataFactory, CommonCore.IKeyVault keyVault)
        {
            _userFactory = userFactory;
            _userCredentialDataFactory = userCredentialDataFactory;
            _userCredentialProcessor = new UserCredentialProcessor(keyVault);
        }

        public async Task<IUser> Authorize(ISettings settings, string name, string password)
        {
            DateTime start = DateTime.UtcNow;
            IUser user = await _userFactory.GetByName(settings, name);
            if (user != null && !user.IsActive)
            {
                user = null;
            }
            else if (user != null && !await CheckUserCredentials(settings, user, password))
            {
                user = null;
            }
            await Sleep(start);
            return user;
        }

        private static async Task Sleep(DateTime start)
        {
            double sleep = DateTime.UtcNow.Subtract(start).TotalSeconds;
            if (sleep > 0.0 && sleep < 1.45)
            {
                await Task.Delay(TimeSpan.FromSeconds(_minimumResponseSeconds - sleep));
            }
        }

        private async Task<bool> CheckUserCredentials(ISettings settings, IUser user, string password)
        {
            IEnumerable<UserCredentialData> credentials = (await _userCredentialDataFactory.GetByUserId(new CommonCore.DataSettings(settings), user.UserId))
                .Where(data => data.IsActive && (!data.Expiration.HasValue || DateTime.UtcNow <= data.Expiration.Value));
            bool isAuthentic = false;
            bool withPasswordless = false;
            int countWithSecret = 0;
            foreach (UserCredentialData credential in credentials)
            {
                if (credential.Secret != null)
                {
                    countWithSecret += 1;
                    isAuthentic = !isAuthentic ? await _userCredentialProcessor.IsAuthentic(settings, credential, password) : isAuthentic;
                }
                else if (string.IsNullOrEmpty(password))
                {
                    withPasswordless = true;
                }
            }
            return isAuthentic || (withPasswordless && countWithSecret == 0);
        }
    }
}
