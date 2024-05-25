using Microsoft.Extensions.Options;

namespace Authorize
{
    public class SettingsFactory : ISettingsFactory
    {
        private readonly IOptions<Settings> _options;

        public SettingsFactory(IOptions<Settings> options)
        {
            _options = options;
        }

        public CoreSettings CreateCore()
            => new CoreSettings(_options.Value);
    }
}
