using Methrax.Bff.AspNetCore.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Methrax.Bff.AspNetCore.Extensions;

internal static class BackendForFrontendOptionsServiceCollectionExtensions
{
    internal static IServiceCollection AddBackendForFrontendOptions(
        this IServiceCollection services,
        Action<BackendForFrontendAuthenticationOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        services
            .AddOptions<BackendForFrontendAuthenticationOptions>()
            .BindConfiguration(BackendForFrontendAuthenticationOptions.SectionName)
            .PostConfigure(options =>
            {
                // Allow consumers to override appsettings values.
                configure?.Invoke(options);
            })
            .ValidateOnStart();

        return services;
    }
}