using BigGrayBison.Authorize.Data.Internal;
using Autofac;

namespace BigGrayBison.Authorize.Data
{
    public class AuthorizeDataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterGeneric(typeof(GenericDataFactory<>))
                .InstancePerLifetimeScope()
                .As(typeof(IGenericDataFactory<>));
            builder.RegisterType<SqlClientProviderFactory>()
                .InstancePerLifetimeScope()
                .As<ISqlDbProviderFactory>()
                .As<IDbProviderFactory>();
            builder.RegisterType<AuthorizationCodeDataFactory>().As<IAuthorizationCodeDataFactory>();
            builder.RegisterType<AuthorizationCodeDataSaver>().As<IAuthorizationCodeDataSaver>();
            builder.RegisterType<ClientDataFactory>().As<IClientDataFactory>();
            builder.RegisterType<SigningKeyDataFactory>().As<ISigningKeyDataFactory>();
            builder.RegisterType<SigningKeyDataSaver>().As<ISigningKeyDataSaver>();
            builder.RegisterType<UserCredentialDataFactory>().As<IUserCredentialDataFactory>();
            builder.RegisterType<UserDataFactory>().As<IUserDataFactory>();
            builder.RegisterType<UserDataSaver>().As<IUserDataSaver>();
        }
    }
}
