using BigGrayBison.Authorize.Data;
using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using BigGrayBison.Authorize.Framework.Enumerations;
using BrassLoon.Interface.Address.Models;
using System;
using System.Threading.Tasks;
using InterfaceAddress = BrassLoon.Interface.Address;

namespace BigGrayBison.Authorize.Core
{
    public class User : IUser
    {
        private readonly UserData _data;
        private readonly IUserDataSaver _dataSaver;
        private readonly InterfaceAddress.IEmailAddressService _emailAddressService;
        private EmailAddress _emailAddress;

        public User(
            UserData data,
            IUserDataSaver dataSaver,
            InterfaceAddress.IEmailAddressService emailAddressService)
        {
            _data = data;
            _dataSaver = dataSaver;
            _emailAddressService = emailAddressService;
        }

        public Guid UserId => _data.UserId;

        public string Name => _data.Name;

        public bool IsActive { get => _data.IsActive; set => _data.IsActive = value; }

        public DateTime CreateTimestamp => _data.CreateTimestamp;

        public DateTime UpdateTimestamp => _data.UpdateTimestamp;

        internal UserRole Role { get => (UserRole)_data.Roles; set => _data.Roles = (short)value; }

        private Guid EmailAddressId
        {
            get => _data.EmailAddressId;
            set => _data.EmailAddressId = value;
        }

        public async Task<string> GetEmailAddress(ISettings settings)
        {
            if (_emailAddress == null && !Guid.Equals(EmailAddressId, Guid.Empty))
                _emailAddress = await _emailAddressService.Get(settings, settings.AddressDomainId.Value, EmailAddressId);
            return _emailAddress?.Address ?? string.Empty;
        }

        public bool IsUserEditor()
            => (Role & UserRole.UserEditor) == UserRole.UserEditor;

        public void IsUserEditor(bool isUserEditor) => SetRole(UserRole.UserEditor, isUserEditor);

        public void SetEmailAddress(string emailAddress)
        {
            _emailAddress = new EmailAddress
            {
                Address = emailAddress
            };
        }

        public async Task Update(ISettings settings, BigGrayBison.Common.Core.ITransactionHandler transactionHandler)
        {
            if (_emailAddress != null && !_emailAddress.EmailAddressId.HasValue)
            {
                _emailAddress.DomainId = settings.AddressDomainId;
                _emailAddress = await _emailAddressService.Save(settings, _emailAddress);
                EmailAddressId = _emailAddress.EmailAddressId.Value;
            }
            await _dataSaver.Update(transactionHandler, _data);
        }

        private void SetRole(UserRole userRole, bool hasRole)
        {
            if (hasRole)
                Role = Role | userRole;
            else
                Role = Role & ~userRole;
        }
    }
}
