using Methrax.Bff.AspNetCore.Options;
using Methrax.Bff.AspNetCore.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Methrax.Bff.AspNetCore.Extensions;

internal static class BackendForFrontendOptionsServiceCollectionExtensions
{
    internal static IServiceCollection AddBackendForFrontendOptions(
        this IServiceCollection services,
        Action<BackendForFrontendAuthenticationOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Register the custom validator in DI
        services.AddSingleton<IValidateOptions<BackendForFrontendAuthenticationOptions>, BackendForFrontendOptionsValidator>();

        // Bind options and enable eager startup validation
        var optionsBuilder = services.AddOptions<BackendForFrontendAuthenticationOptions>()
            .BindConfiguration(BackendForFrontendAuthenticationOptions.SectionName);

        if (configure is not null)
        {
            optionsBuilder.Configure(configure);
        }

        optionsBuilder.ValidateOnStart();

        return services;
    }
}