using Autofac;
namespace Authorize
{
    public class AuthorizeApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new BigGrayBison.Authorize.Core.AuthorizeCodeModule());
            builder.RegisterType<SettingsFactory>()
                .InstancePerLifetimeScope()
                .As<ISettingsFactory>();
        }
    }
}
