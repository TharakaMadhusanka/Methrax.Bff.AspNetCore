using Methrax.Bff.AspNetCore.Options;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Methrax.Bff.AspNetCore.Authentication
{
    internal static class OpenIdConnectConfiguration
    {
        internal static IServiceCollection ConfigureOpenIdConnect(
            this IServiceCollection services)
        {
            services
                .AddOptions<OpenIdConnectOptions>(AuthenticationDefaults.OpenIdConnectScheme)
                .Configure<IOptions<BackendForFrontendAuthenticationOptions>>((oidc, bff) =>
                {
                    var cfg = bff.Value;

                    oidc.Authority = cfg.Authority;
                    oidc.ClientId = cfg.ClientId;
                    oidc.ClientSecret = cfg.ClientSecret;

                    oidc.RequireHttpsMetadata = cfg.RequireHttpsMetadata;

                    oidc.ResponseType = OpenIdConnectResponseType.Code;
                    oidc.SaveTokens = false;

                    oidc.Scope.Clear();

                    foreach (var scope in cfg.Scopes)
                    {
                        oidc.Scope.Add(scope);
                    }
                });

            return services;
        }
    }
}
