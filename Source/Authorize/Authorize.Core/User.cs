using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using BrassLoon.Interface.Address.Models;
using System;
using System.Threading.Tasks;
using InterfaceAddress = BrassLoon.Interface.Address;

namespace BigGrayBison.Authorize.Core
{
    public class User : IUser
    {
        private readonly UserData _data;
        private readonly InterfaceAddress.IEmailAddressService _emailAddressService;
        private EmailAddress _emailAddress;

        public User(UserData data, InterfaceAddress.IEmailAddressService emailAddressService)
        {
            _data = data;
            _emailAddressService = emailAddressService;
        }

        public Guid UserId => _data.UserId;

        public string Name => _data.Name;

        public bool IsActive { get => _data.IsActive; set => _data.IsActive = value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public DateTime UpdateTimestamp => _data.UpdateTimestamp;

        private Guid EmailAddressId => _data.EmailAddressId;

        public async Task<string> GetEmailAddress(ISettings settings)
        {
            if (_emailAddress == null && !Guid.Equals(EmailAddressId, Guid.Empty))
                _emailAddress = await _emailAddressService.Get(settings, settings.AddressDomainId.Value, EmailAddressId);
            return _emailAddress?.Address ?? string.Empty;
        }

        public void SetEmailAddress(string emailAddress)
        {
            _emailAddress = new EmailAddress
            {
                Address = emailAddress
            };
        }
    }
}
