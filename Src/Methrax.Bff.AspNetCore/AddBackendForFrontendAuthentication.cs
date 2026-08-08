using Methrax.Bff.AspNetCore.Authentication;
using Methrax.Bff.AspNetCore.Extensions;
using Methrax.Bff.AspNetCore.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Methrax.Bff.AspNetCore;

/// <summary>
/// Extension methods for setting up Backend-for-Frontend (BFF) authentication services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class AddBackendForFrontendAuthentication
{
    /// <summary>
    /// Adds and configures Backend-for-Frontend (BFF) authentication services, setting up Cookie Authentication 
    /// and OpenID Connect (OIDC) handlers with secure defaults.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configure">An optional delegate to configure <see cref="BackendForFrontendAuthenticationOptions"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/> is <c>null</c>.</exception>
    public static IServiceCollection AddBffAuthentication(
        this IServiceCollection services,
        Action<BackendForFrontendAuthenticationOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        services
            .AddBackendForFrontendOptions(configure)
            .ConfigureCookieAuthentication()
            .ConfigureOpenIdConnect()
            .AddBackEndForFrontendAuthentication();

        return services;
    }
}