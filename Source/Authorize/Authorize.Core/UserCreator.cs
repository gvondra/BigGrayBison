using BigGrayBison.Authorize.Common.Exception;
using BigGrayBison.Authorize.Data;
using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using BrassLoon.Interface.Address.Models;
using System;
using System.Threading.Tasks;
using CommonCore = BigGrayBison.Common.Core;
using InterfaceAddress = BrassLoon.Interface.Address;

namespace BigGrayBison.Authorize.Core
{
    public sealed class UserCreator : IUserCreator
    {
        private readonly IUserValidator _userValidator;
        private readonly InterfaceAddress.IEmailAddressService _emailAddressService;
        private readonly IUserDataSaver _dataSaver;
        private readonly UserCredentialProcessor _userCredentialProcessor;
        
        public UserCreator(
            IUserValidator userValidator,
            InterfaceAddress.IEmailAddressService emailAddressService,
            CommonCore.IKeyVault keyVault,
            IUserDataSaver dataSaver)
        {
            _userValidator = userValidator;
            _emailAddressService = emailAddressService;
            _userCredentialProcessor = new UserCredentialProcessor(keyVault);
            _dataSaver = dataSaver;
        }

        public async Task<IUser> Create(ISettings settings, string userName, string password, string emailAddress)
        {
            Validate(userName, password, emailAddress);
            EmailAddress email = await _emailAddressService.Save(settings, new EmailAddress { DomainId = settings.AddressDomainId.Value, Address = emailAddress });

            UserData data = new UserData
            {
                Name = userName,
                EmailAddressId = email.EmailAddressId.Value,
                IsActive = true
            };
            UserCredentialData userCredentialData = await _userCredentialProcessor.Create(settings, password);
            await CommonCore.Saver.Save(
                new CommonCore.TransactionHandler(settings),
                th => _dataSaver.Create(th, data, userCredentialData));
            return new User(data, _dataSaver, _emailAddressService);
        }

        private void Validate(string userName, string password, string emailAddress)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(userName);
            ArgumentNullException.ThrowIfNullOrEmpty(password);
            ArgumentNullException.ThrowIfNullOrEmpty(emailAddress);

            string message;
            message = _userValidator.ValidateUserName(userName);
            if (!string.IsNullOrEmpty(message))
                throw new InvalidUserException(message);
            message = _userValidator.ValidatePassword(password);
            if (!string.IsNullOrEmpty(message))
                throw new InvalidUserException(message);
        }
    }
}
