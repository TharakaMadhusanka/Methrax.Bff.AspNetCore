using Methrax.Bff.AspNetCore.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                .Configure<IOptions<BackendForFrontendAuthenticationOptions>, ILoggerFactory>((oidc, bff, loggerFactory) =>
                {
                    var cfg = bff.Value;
                    var logger = loggerFactory.CreateLogger("Methrax.Bff.AspNetCore");
                    logger.LogInformation("Configuring OpenID Connect options for scheme: {Scheme}", AuthenticationDefaults.OpenIdConnectScheme);

                    oidc.Authority = cfg.Authority;
                    oidc.ClientId = cfg.ClientId;
                    oidc.ClientSecret = cfg.ClientSecret;

                    oidc.RequireHttpsMetadata = cfg.RequireHttpsMetadata;

                    oidc.ResponseType = OpenIdConnectResponseType.Code;
                    oidc.SaveTokens = cfg.SaveTokens;

                    oidc.Scope.Clear();

                    foreach (var scope in cfg.Scopes)
                    {
                        oidc.Scope.Add(scope);
                    }

                    oidc.Events.OnTokenValidated = context =>
                    {
                        if (cfg.SaveTokens && context.TokenEndpointResponse != null && context.Properties != null)
                        {
                            var tokenNames = cfg.Tokens;
                            var tokens = new List<AuthenticationToken>();

                            if (!string.IsNullOrEmpty(context.TokenEndpointResponse.AccessToken))
                            {
                                tokens.Add(new AuthenticationToken
                                {
                                    Name = tokenNames.AccessToken,
                                    Value = context.TokenEndpointResponse.AccessToken
                                });
                            }

                            if (!string.IsNullOrEmpty(context.TokenEndpointResponse.RefreshToken))
                            {
                                tokens.Add(new AuthenticationToken
                                {
                                    Name = tokenNames.RefreshToken,
                                    Value = context.TokenEndpointResponse.RefreshToken
                                });
                            }

                            if (!string.IsNullOrEmpty(context.TokenEndpointResponse.IdToken))
                            {
                                tokens.Add(new AuthenticationToken
                                {
                                    Name = tokenNames.IdToken,
                                    Value = context.TokenEndpointResponse.IdToken
                                });
                            }

                            if (!string.IsNullOrEmpty(context.TokenEndpointResponse.ExpiresIn) &&
                                double.TryParse(context.TokenEndpointResponse.ExpiresIn, out var expiresInSeconds))
                            {
                                var baseTime = cfg.UseUtcTimeZone
                                    ? DateTimeOffset.UtcNow
                                    : DateTimeOffset.Now;

                                var expiresAt = baseTime
                                    .AddSeconds(expiresInSeconds)
                                    .ToString("o");

                                tokens.Add(new AuthenticationToken
                                {
                                    Name = tokenNames.ExpiresAt,
                                    Value = expiresAt
                                });
                            }

                            context.Properties.StoreTokens(tokens);
                        }
                        return Task.CompletedTask;
                    };
                });

            return services;
        }
    }
}
