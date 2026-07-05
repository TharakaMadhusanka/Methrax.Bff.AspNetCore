using Methrax.Bff.AspNetCore.Authentication;
using Methrax.Bff.AspNetCore.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Methrax.Bff.AspNetCore.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBffAuthentication(
        this IServiceCollection services,
        Action<BackendForFrontendAuthenticationOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        services
            .AddBackendForFrontendOptions(configure)
            .AddBackEndForFrontendAuthentication()
            .ConfigureCookieAuthentication()
            .ConfigureOpenIdConnect();

        return services;
    }
}