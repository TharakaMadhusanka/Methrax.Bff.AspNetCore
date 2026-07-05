using Methrax.Bff.AspNetCore.Authentication;
using Methrax.Bff.AspNetCore.Extensions;
using Methrax.Bff.AspNetCore.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Methrax.Bff.AspNetCore;

public static class AddBackendForFrontendAuthentication
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