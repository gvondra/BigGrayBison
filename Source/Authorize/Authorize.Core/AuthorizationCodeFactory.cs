using BigGrayBison.Authorize.Data;
using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonCore = BigGrayBison.Common.Core;

namespace BigGrayBison.Authorize.Core
{
    public class AuthorizationCodeFactory : IAuthorizationCodeFactory
    {
        private static readonly Policy _keyCache = Policy.Cache(new MemoryCacheProvider(new MemoryCache(new MemoryCacheOptions())), TimeSpan.FromHours(4));
        private readonly IAuthorizationCodeDataFactory _dataFactory;
        private readonly IAuthorizationCodeDataSaver _dataSaver;
        private readonly CommonCore.IKeyVault _keyVault;
        private readonly IUserFactory _userFactory;

        public AuthorizationCodeFactory(
            IAuthorizationCodeDataFactory dataFactory,
            IAuthorizationCodeDataSaver dataSaver,
            CommonCore.IKeyVault keyVault,
            IUserFactory userFactory)
        {
            _dataFactory = dataFactory;
            _dataSaver = dataSaver;
            _keyVault = keyVault;
            _userFactory = userFactory;
        }

        public IAuthorizationCode Create(ISettings settings, IClient client, IUser user, string state, string redirectUrl)
        {
            return new AuthorizationCode(new Data.Models.AuthorizationCodeData(), _dataSaver, _keyVault, _userFactory, user, client)
            {
                KeyId = GetKey(),
                State = state,
                Expiration = DateTime.UtcNow.AddSeconds(90),
                RedirectUrl = redirectUrl,
                IsActive = true
            };
        }

        public async Task<IEnumerable<IAuthorizationCode>> GetByClientIdIsActiveMinExpiration(ISettings settings, Guid clientId, bool isActive, DateTime minExpiration)
        {
            return (await _dataFactory.GetByClientIdIsActiveMinExpiration(new CommonCore.DataSettings(settings), clientId, isActive, minExpiration))
                .Select<AuthorizationCodeData, IAuthorizationCode>(Create);
        }

        public async Task<IAuthorizationCode> GetActiveByCode(ISettings settings, Guid clientId, string code)
        {
            return (await Task.WhenAll((await GetByClientIdIsActiveMinExpiration(settings, clientId, true, DateTime.UtcNow))
                .Select(authCode => IsCodeMatch(settings, authCode, code))))
                .Where(v => v.Item2)
                .Select(v => v.Item1)
                .SingleOrDefault();
        }

        private static async Task<(IAuthorizationCode, bool)> IsCodeMatch(ISettings settings, IAuthorizationCode authorizationCode, string code)
        {
            byte[] bytes = await authorizationCode.GetCode(settings);
            return (authorizationCode,
                string.Equals(
                    code,
                    string.Join(string.Empty, bytes.Select(x => x.ToString("x2"))),
                    StringComparison.OrdinalIgnoreCase));
        }

        private static Guid GetKey()
        {
            return _keyCache.Execute(
                context => Guid.NewGuid(),
                new Context());
        }

        private AuthorizationCode Create(AuthorizationCodeData data) => new AuthorizationCode(data, _dataSaver, _keyVault, _userFactory);
    }
}
