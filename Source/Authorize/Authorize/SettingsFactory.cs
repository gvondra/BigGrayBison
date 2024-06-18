using BrassLoon.Interface.Account;
using Microsoft.Extensions.Options;

namespace Authorize
{
    public class SettingsFactory : ISettingsFactory
    {
        private readonly IOptions<Settings> _options;
        private readonly ITokenService _tokenService;

        public SettingsFactory(IOptions<Settings> options, ITokenService tokenService)
        {
            _options = options;
            _tokenService = tokenService;
        }

        public CoreSettings CreateCore()
            => new CoreSettings(_options.Value, _tokenService);
    }
}
