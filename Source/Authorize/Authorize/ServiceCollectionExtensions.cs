using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authorize
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            string idIssuer = configuration["TokenIssuer"];
            _ = services.AddAuthorization(o =>
            {
                o.DefaultPolicy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
                o.AddPolicy(
                    Constaints.POLICY_EDIT_USER,
                    configure =>
                    {
                        _ = configure.AddRequirements(new AuthorizationRequirement(Constaints.POLICY_EDIT_USER, idIssuer, Constaints.POLICY_EDIT_USER));
                    });
            });
            return services;
        }
    }
}
