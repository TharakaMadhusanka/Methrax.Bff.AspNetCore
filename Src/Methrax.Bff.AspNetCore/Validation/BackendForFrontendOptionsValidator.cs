using Methrax.Bff.AspNetCore.Options;
using Microsoft.Extensions.Options;

namespace Methrax.Bff.AspNetCore.Validation;

/// <summary>
/// Validates <see cref="BackendForFrontendAuthenticationOptions"/> at application startup 
/// to ensure required OIDC settings, cookies, and endpoint configurations are present and valid.
/// </summary>
public class BackendForFrontendOptionsValidator : IValidateOptions<BackendForFrontendAuthenticationOptions>
{
    /// <inheritdoc />
    public ValidateOptionsResult Validate(string? name, BackendForFrontendAuthenticationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var failures = new List<string>();

        // Validate Identity Provider (OIDC) settings
        if (string.IsNullOrWhiteSpace(options.Authority))
        {
            failures.Add($"'{nameof(options.Authority)}' must be provided.");
        }
        else if (!Uri.TryCreate(options.Authority, UriKind.Absolute, out _))
        {
            failures.Add($"'{nameof(options.Authority)}' must be a valid absolute URL.");
        }

        if (string.IsNullOrWhiteSpace(options.ClientId))
        {
            failures.Add($"'{nameof(options.ClientId)}' cannot be empty or null.");
        }

        if (string.IsNullOrWhiteSpace(options.ClientSecret))
        {
            failures.Add($"'{nameof(options.ClientSecret)}' cannot be empty or null.");
        }

        // Validate Endpoint Paths
        if (options.Endpoints is not null)
        {
            ValidateRelativePath(options.Endpoints.LoginPath, nameof(options.Endpoints.LoginPath), failures);
            ValidateRelativePath(options.Endpoints.LogoutPath, nameof(options.Endpoints.LogoutPath), failures);
            ValidateRelativePath(options.Endpoints.AccessDeniedPath, nameof(options.Endpoints.AccessDeniedPath), failures);
        }
        else
        {
            failures.Add($"'{nameof(options.Endpoints)}' section cannot be null.");
        }

        // Validate Token Keys Configuration
        if (options.Tokens is not null)
        {
            if (string.IsNullOrWhiteSpace(options.Tokens.AccessToken))
            {
                failures.Add($"'{nameof(options.Tokens.AccessToken)}' key name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(options.Tokens.RefreshToken))
            {
                failures.Add($"'{nameof(options.Tokens.RefreshToken)}' key name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(options.Tokens.IdToken))
            {
                failures.Add($"'{nameof(options.Tokens.IdToken)}' key name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(options.Tokens.ExpiresAt))
            {
                failures.Add($"'{nameof(options.Tokens.ExpiresAt)}' key name cannot be empty.");
            }
        }
        else
        {
            failures.Add($"'{nameof(options.Tokens)}' section cannot be null.");
        }

        // Return aggregated results
        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;
    }

    private static void ValidateRelativePath(string? path, string propertyName, List<string> failures)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            failures.Add($"'{propertyName}' cannot be empty or null.");
        }
        else if (!path.StartsWith('/'))
        {
            failures.Add($"'{propertyName}' must start with a leading slash '/' (value was: '{path}').");
        }
    }
}