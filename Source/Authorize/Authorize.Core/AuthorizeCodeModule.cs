using Autofac;
using BigGrayBison.Authorize.Framework;

namespace BigGrayBison.Authorize.Core
{
    public class AuthorizeCodeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new BigGrayBison.Authorize.Data.AuthorizeDataModule());
            builder.RegisterType<ClientFactory>().As<IClientFactory>();
        }
    }
}
