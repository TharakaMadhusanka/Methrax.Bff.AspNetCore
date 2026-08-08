using Microsoft.Extensions.DependencyInjection;

namespace Methrax.Bff.AspNetCore.Authentication
{
    internal static class AuthenticationServiceCollectionExtensions
    {
        internal static IServiceCollection AddBackEndForFrontendAuthentication(this IServiceCollection services)
        {
            services
                .AddLogging()
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = AuthenticationDefaults.CookieScheme;
                    options.DefaultChallengeScheme = AuthenticationDefaults.OpenIdConnectScheme;
                })
            .AddCookie(AuthenticationDefaults.CookieScheme)
            .AddOpenIdConnect(AuthenticationDefaults.OpenIdConnectScheme, (options) => { });

            return services;
        }
    }
}
