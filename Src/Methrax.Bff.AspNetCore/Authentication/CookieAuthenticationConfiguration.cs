using Methrax.Bff.AspNetCore.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Methrax.Bff.AspNetCore.Authentication;

internal static class CookieAuthenticationConfiguration
{
    internal static IServiceCollection ConfigureCookieAuthentication(
        this IServiceCollection services)
    {
        services
        .AddOptions<CookieAuthenticationOptions>(AuthenticationDefaults.CookieScheme)
        .Configure<IOptions<BackendForFrontendAuthenticationOptions>>((cookie, cfg) =>
        {
            var options = cfg.Value;

            cookie.LoginPath = options.Endpoints.LoginPath;
            cookie.LogoutPath = options.Endpoints.LogoutPath;
            cookie.AccessDeniedPath = options.Endpoints.AccessDeniedPath;

            cookie.Cookie.HttpOnly = options.Cookie.HttpOnly;
            cookie.Cookie.SameSite = options.Cookie.SameSite;
            cookie.Cookie.SecurePolicy = options.Cookie.SecurePolicy;

            cookie.Events.OnRedirectToLogin = context =>
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }

                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            };
        });

        return services;
    }
}