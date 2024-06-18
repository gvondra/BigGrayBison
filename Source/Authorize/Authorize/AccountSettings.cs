using BrassLoon.Interface.Account;
using System.Threading.Tasks;

namespace Authorize
{
    public class AccountSettings : ISettings
    {
        private readonly Settings _settings;

        public AccountSettings(Settings settings)
        {
            _settings = settings;
        }

        public string BaseAddress => _settings.BrassLoonAccountApiBaseAddress;

        public Task<string> GetToken()
        {
            throw new System.NotImplementedException();
        }
    }
}
