using System;
using System.Threading.Tasks;

namespace Authorize
{
    public class CoreSettings : BigGrayBison.Common.Core.ISettings
    {
        private readonly Settings _settings;

        public CoreSettings(Settings settings)
        {
            _settings = settings;
        }

        public bool UseDefaultAzureSqlToken => _settings.EnableDatabaseAccessToken;

        public Task<string> GetConnetionString() => Task.FromResult(_settings.ConnectionString);

        public Func<Task<string>> GetDatabaseAccessToken() => null;
    }
}
