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
            builder.RegisterType<ClientDataFactory>().As<IClientDataFactory>();
        }
    }
}
