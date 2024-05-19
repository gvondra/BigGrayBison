using Autofac;
using Autofac.Extensions.DependencyInjection;
using BrassLoon.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Authorize
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer((ContainerBuilder builder) => builder.RegisterModule(new AuthorizeApiModule()));

            // Add services to the container.
            builder.Services.Configure<Settings>(builder.Configuration);

            builder.Services.AddLogging(b =>
            {
                b.ClearProviders();
                b.AddConsole();
                Settings settings = new Settings();
                builder.Configuration.Bind(settings);
                if (settings.LogDomainId.HasValue && !string.IsNullOrEmpty(settings.BrassLoonLogRpcBaseAddress) && settings.BrassLoonClientId.HasValue)
                {
                    b.AddBrassLoonLogger(c =>
                    {
                        c.LogApiBaseAddress = settings.BrassLoonLogRpcBaseAddress;
                        c.LogDomainId = settings.LogDomainId.Value;
                        c.LogClientId = settings.BrassLoonClientId.Value;
                        c.LogClientSecret = settings.BrassLoonClientSecret;
                    });
                }
            });

            builder.Services.AddControllersWithViews();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
