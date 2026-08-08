using Methrax.Bff.AspNetCore.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Methrax.Bff.AspNetCore.Authentication;

internal static class CookieAuthenticationConfiguration
{
    internal static IServiceCollection ConfigureCookieAuthentication(
        this IServiceCollection services)
    {
        // Required for the cookie authentication scheme to work properly with server-side sessions.
        services
        .AddOptions<CookieAuthenticationOptions>(AuthenticationDefaults.CookieScheme)
        .Configure<IOptions<BackendForFrontendAuthenticationOptions>, ILoggerFactory>((cookie, cfg, loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("Methrax.Bff.AspNetCore");
            logger.LogInformation("Configuring cookie authentication options for scheme: {Scheme}", AuthenticationDefaults.CookieScheme);

            var options = cfg.Value;

            cookie.LoginPath = options.Endpoints.LoginPath;
            cookie.LogoutPath = options.Endpoints.LogoutPath;
            cookie.AccessDeniedPath = options.Endpoints.AccessDeniedPath;

            cookie.Cookie.HttpOnly = options.Cookie.HttpOnly;
            cookie.Cookie.SameSite = options.Cookie.SameSite;
            cookie.Cookie.SecurePolicy = options.Cookie.SecurePolicy;

            cookie.Events.OnRedirectToLogin = context =>
            {
                // Restrict login for /api endpoints to return 401 Unauthorized instead of redirecting to the login page.
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }

                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            };
        });

        // Optionally configure the cookie authentication options to use a server-side session store if enabled in the BFF options.
        services.AddOptions<CookieAuthenticationOptions>(AuthenticationDefaults.CookieScheme)
            .PostConfigure<IOptions<BackendForFrontendAuthenticationOptions>, IEnumerable<ITicketStore>, ILoggerFactory>(
                (cookie, bffOptionsMonitor, ticketStores, loggerFactory) =>
                {
                    var options = bffOptionsMonitor.Value;

                    if (options.EnableServerSideSessions)
                    {
                        var ticketStore = ticketStores.FirstOrDefault();

                        if (ticketStore is null)
                        {
                            var logger = loggerFactory.CreateLogger("Methrax.Bff.AspNetCore");

                            logger.LogError(
                                "Failed to configure cookie scheme '{Scheme}'. Server-side sessions are enabled ('EnableServerSideSessions = true'), " +
                                "but no implementation of 'ITicketStore' was registered in DI.",
                                AuthenticationDefaults.CookieScheme);

                            throw new InvalidOperationException(
                                "Server-side sessions are enabled ('EnableServerSideSessions = true'), " +
                                "but no implementation of 'ITicketStore' was registered in the dependency injection container. " +
                                "Please register an ITicketStore implementation (e.g., MemoryCacheTicketStore or RedisTicketStore) " +
                                "or set EnableServerSideSessions to false.");
                        }

                        cookie.SessionStore = ticketStore;
                    }
                });

        return services;
    }
}