namespace Methrax.Bff.AspNetCore.Options;

/// <summary>
/// Represents the configuration options for the Backend-for-Frontend (BFF).
/// </summary>
public sealed class BackendForFrontendAuthenticationOptions
{
    /// <summary>
    /// The configuration section name.
    /// </summary>
    public const string SectionName = "BffAuthentication";

    /// <summary>
    /// Gets or sets the OpenID Connect authority.
    /// </summary>
    public string Authority { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the client identifier.
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the client secret.
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether HTTPS metadata is required.
    /// </summary>
    public bool RequireHttpsMetadata { get; set; } = true;

    /// <summary>
    /// Cookie settings.
    /// </summary>
    public CookieOptions Cookie { get; } = new();

    /// <summary>
    /// Authentication endpoint settings.
    /// </summary>
    public AuthenticationEndpoints Endpoints { get; } = new();

    /// <summary>
    /// Requested OpenID Connect scopes.
    /// </summary>
    public IList<string> Scopes { get; } =
    [
        "openid",
        "profile",
        "offline_access"
    ];
}