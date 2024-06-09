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
            builder.RegisterType<ClientFactory>().As<IClientFactory>();
            builder.RegisterType<KeyVault>().As<IKeyVault>();
            builder.RegisterType<SigningKeyFactory>().As<ISigningKeyFactory>();
            builder.RegisterType<SigningKeySaver>().As<ISigningKeySaver>();
            builder.RegisterType<UserAuthorizer>().As<IUserAuthorizer>();
            builder.RegisterType<UserFactory>().As<IUserFactory>();
        }
    }
}
