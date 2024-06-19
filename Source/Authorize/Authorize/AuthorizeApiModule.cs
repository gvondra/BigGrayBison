using Autofac;
namespace Authorize
{
    public class AuthorizeApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new BigGrayBison.Authorize.Core.AuthorizeCodeModule());
            builder.RegisterModule(new BrassLoon.Interface.Account.AccountInterfaceModule());
            builder.RegisterModule(new BrassLoon.Interface.Address.AddressInterfaceModule());
            builder.RegisterType<SettingsFactory>()
                .InstancePerLifetimeScope()
                .As<ISettingsFactory>();
        }
    }
}
