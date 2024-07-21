using Autofac;
using BigGrayBison.Authorize.Framework;
using BigGrayBison.Common.Core;

namespace BigGrayBison.Authorize.Core
{
    public class AuthorizeCodeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new BigGrayBison.Authorize.Data.AuthorizeDataModule());
            builder.RegisterType<AccessTokenGenerator>().As<IAccessTokenGenerator>();
            builder.RegisterType<AuthorizationCodeFactory>().As<IAuthorizationCodeFactory>();
            builder.RegisterType<AuthorizationCodeSaver>().As<IAuthorizationCodeSaver>();
            builder.RegisterType<ClientFactory>().As<IClientFactory>();
            builder.RegisterType<KeyVault>().As<IKeyVault>();
            builder.RegisterType<SigningKeyFactory>().As<ISigningKeyFactory>();
            builder.RegisterType<SigningKeySaver>().As<ISigningKeySaver>();
            builder.RegisterType<UserCreator>().As<IUserCreator>();
            builder.RegisterType<UserCredentialUpdater>().As<IUserCredentialUpdater>();
            builder.RegisterType<UserAuthorizer>().As<IUserAuthorizer>();
            builder.RegisterType<UserCreator>().As<IUserCreator>();
            builder.RegisterType<UserFactory>().As<IUserFactory>();
            builder.RegisterType<UserUpdater>().As<IUserUpdater>();
            builder.RegisterType<UserValidator>().As<IUserValidator>();
        }
    }
}
