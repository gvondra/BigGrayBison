using Azure.Security.KeyVault.Keys;
using BigGrayBison.Authorize.Core;
using BigGrayBison.Authorize.Data;
using BigGrayBison.Authorize.Data.Models;
using BigGrayBison.Authorize.Framework;
using BrassLoon.Interface.Address.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CommonCore = BigGrayBison.Common.Core;
using DataClient = BrassLoon.DataClient;
using InterfaceAddress = BrassLoon.Interface.Address;

namespace Authorize.Core.Test
{
    [TestClass]
    public class CreateUserTest
    {
        private const string _userName = "test-user-name";
        [TestMethod]
        public async Task CreateTest()
        {
            string secret = "ZYX123xyz!@#" + new string('0', 1024);
            string emailAddress = "test@test.test";
            UserCredentialData userCredentialData = null;

            Mock<ISettings> settings = CreateSettings();
            UserValidator validator = new UserValidator();
            Mock<InterfaceAddress.IEmailAddressService> emailAddressService = CreateEmailAddressService();
            Mock<CommonCore.IKeyVault> keyVault = CreateKeyVault();
            Mock<IUserDataSaver> userDataSaver = new Mock<IUserDataSaver>();
            userDataSaver.Setup(s => s.Create(It.IsNotNull<DataClient.ITransactionHandler>(), It.IsNotNull<UserData>(), It.IsNotNull<UserCredentialData>()))
                .Returns((DataClient.ITransactionHandler th, UserData ud, UserCredentialData ucd) =>
                {
                    userCredentialData = ucd;
                    return Task.CompletedTask;
                });

            UserCreator userCreator = new UserCreator(validator, emailAddressService.Object, keyVault.Object, userDataSaver.Object);
            IUser result = await userCreator.Create(settings.Object, _userName, secret, emailAddress);
            Assert.IsNotNull(result);

            emailAddressService.Verify(s => s.Save(It.IsNotNull<InterfaceAddress.ISettings>(), It.IsNotNull<EmailAddress>()), Times.Once);
            userDataSaver.Verify(s => s.Create(It.IsNotNull<DataClient.ITransactionHandler>(), It.IsNotNull<UserData>(), It.IsNotNull<UserCredentialData>()), Times.Once);

            await AuthenticateUserTest(userCredentialData, secret, keyVault);

            keyVault.Verify(v => v.GetKey(It.IsAny<string>(), It.IsNotNull<string>(), default(DateTime?)), Times.Never);
            keyVault.Verify(v => v.Encrypt(It.IsAny<string>(), It.IsNotNull<string>(), It.IsNotNull<byte[]>(), default(DateTime?)), Times.Once);
            keyVault.Verify(v => v.Decrypt(It.IsAny<string>(), It.IsNotNull<string>(), It.IsNotNull<byte[]>()), Times.Once);
        }

        private async Task AuthenticateUserTest(UserCredentialData userCredentialData, string secret, Mock<CommonCore.IKeyVault> keyVault)
        {
            Guid userId = Guid.NewGuid();
            Mock<ISettings> settings = CreateSettings();
            Mock<IUserFactory> userFactory = new Mock<IUserFactory>();
            userFactory.Setup(f => f.GetByName(It.IsAny<ISettings>(), _userName))
                .Returns(() =>
                {
                    Mock<IUser> user = new Mock<IUser>();
                    user.SetupAllProperties();
                    user.Object.IsActive = true;
                    user.SetupGet(u => u.UserId).Returns(userId);
                    return Task.FromResult(user.Object);
                });

            Mock<IUserCredentialDataFactory> credentialDataFactory = new Mock<IUserCredentialDataFactory>();
            credentialDataFactory.Setup(f => f.GetByUserId(It.IsAny<DataClient.ISqlSettings>(), userId))
                .Returns(() => Task.FromResult<IEnumerable<UserCredentialData>>(new List<UserCredentialData> { userCredentialData }));

            UserAuthorizer userAuthorizer = new UserAuthorizer(userFactory.Object, credentialDataFactory.Object, keyVault.Object);
            Assert.IsNotNull(await userAuthorizer.Authorize(settings.Object, _userName, secret));
            credentialDataFactory.Verify(f => f.GetByUserId(It.IsNotNull<DataClient.ISqlSettings>(), userId), Times.Once);
        }

        private static Mock<CommonCore.IKeyVault> CreateKeyVault()
        {
            Mock<CommonCore.IKeyVault> keyVault = new Mock<CommonCore.IKeyVault>();
            //keyVault.Setup(v => v.GetKey(It.IsAny<string>(), It.IsNotNull<string>()))
            //    .Returns(Task.FromResult(new JsonWebKey(RSA.Create(2048), true, new List<KeyOperation> { KeyOperation.Encrypt, KeyOperation.Decrypt })));
            keyVault.Setup(v => v.Encrypt(It.IsAny<string>(), It.IsNotNull<string>(), It.IsNotNull<byte[]>(), default(DateTime?)))
                .Returns((string url, string key, byte[] value, DateTime? e) =>
                {
                    return Task.FromResult(value);
                });
            keyVault.Setup(v => v.Decrypt(It.IsAny<string>(), It.IsNotNull<string>(), It.IsNotNull<byte[]>()))
                .Returns((string url, string key, byte[] value) =>
                {
                    return Task.FromResult(value);
                });
            return keyVault;
        }

        private static Mock<InterfaceAddress.IEmailAddressService> CreateEmailAddressService()
        {
            Mock<InterfaceAddress.IEmailAddressService> emailAddressService = new Mock<InterfaceAddress.IEmailAddressService>();
            emailAddressService.Setup(s => s.Save(It.IsNotNull<InterfaceAddress.ISettings>(), It.IsNotNull<EmailAddress>()))
                .Returns((InterfaceAddress.ISettings s, EmailAddress ea) =>
                {
                    ea.EmailAddressId ??= Guid.NewGuid();
                    return Task.FromResult(ea);
                });
            return emailAddressService;
        }

        private Mock<ISettings> CreateSettings()
        {
            Mock<ISettings> settings = new Mock<ISettings>();
            settings.SetupGet(s => s.AddressDomainId).Returns(Guid.NewGuid());
            return settings;
        }
    }
}
