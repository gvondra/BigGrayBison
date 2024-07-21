using BigGrayBison.Authorize.Data;
using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using System;
using System.Threading.Tasks;
using CommonCore = BigGrayBison.Common.Core;
using InterfaceAddress = BrassLoon.Interface.Address;

namespace BigGrayBison.Authorize.Core
{
    public class UserFactory : IUserFactory
    {
        private readonly IUserDataFactory _dataFactory;
        private readonly IUserDataSaver _userDataSaver;
        private readonly InterfaceAddress.IEmailAddressService _emailAddressService;

        public UserFactory(IUserDataFactory dataFactory, IUserDataSaver dataSaver, InterfaceAddress.IEmailAddressService emailAddressService)
        {
            _dataFactory = dataFactory;
            _userDataSaver = dataSaver;
            _emailAddressService = emailAddressService;
        }

        public async Task<IUser> Get(ISettings settings, Guid id)
        {
            UserData data = await _dataFactory.Get(new CommonCore.DataSettings(settings), id);
            return data != null ? Create(data) : null;
        }

        public async Task<IUser> GetByName(ISettings settings, string name)
        {
            UserData userData = await _dataFactory.GetByName(new CommonCore.DataSettings(settings), name);
            return userData != null ? Create(userData) : null;
        }

        public Task<bool> GetUserNameAvailable(ISettings settings, string name)
            => _dataFactory.GetUserNameAvailable(new CommonCore.DataSettings(settings), name);

        private User Create(UserData data) => new User(data, _userDataSaver, _emailAddressService);
    }
}
