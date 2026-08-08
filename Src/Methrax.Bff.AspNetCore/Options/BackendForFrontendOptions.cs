using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Methrax.Bff.AspNetCore.Options;

/// <summary>
/// Provides default constant key names used for storing and retrieving OAuth2 and OpenID Connect 
/// tokens within authentication properties.
/// </summary>
public static class BffTokenKeys
{
    /// <summary>
    /// The default key name for the OAuth2 access token ("access_token").
    /// Maps to <see cref="Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectParameterNames.AccessToken"/>.
    /// </summary>
    public const string AccessToken = OpenIdConnectParameterNames.AccessToken;

    /// <summary>
    /// The default key name for the OAuth2 refresh token ("refresh_token").
    /// Maps to <see cref="Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectParameterNames.RefreshToken"/>.
    /// </summary>
    public const string RefreshToken = OpenIdConnectParameterNames.RefreshToken;

    /// <summary>
    /// The default key name for the OpenID Connect ID token ("id_token").
    /// Maps to <see cref="Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectParameterNames.IdToken"/>.
    /// </summary>
    public const string IdToken = OpenIdConnectParameterNames.IdToken;

    /// <summary>
    /// The default key name for the token expiration ISO-8601 UTC timestamp string ("expires_at").
    /// </summary>
    public const string ExpiresAt = "expires_at";
}

/// <summary>
/// Configures the property key names used when storing and retrieving OAuth2/OIDC tokens 
/// within ASP.NET Core <see cref="Microsoft.AspNetCore.Authentication.AuthenticationProperties"/>.
/// </summary>
public sealed class BffTokenOptions
{
    /// <summary>
    /// Gets or sets the key name used to store the OAuth2 access token.
    /// Defaults to <see cref="BffTokenKeys.AccessToken"/> ("access_token").
    /// </summary>
    public string AccessToken { get; set; } = BffTokenKeys.AccessToken;

    /// <summary>
    /// Gets or sets the key name used to store the OAuth2 refresh token.
    /// Defaults to <see cref="BffTokenKeys.RefreshToken"/> ("refresh_token").
    /// </summary>
    public string RefreshToken { get; set; } = BffTokenKeys.RefreshToken;

    /// <summary>
    /// Gets or sets the key name used to store the OpenID Connect ID token.
    /// Defaults to <see cref="BffTokenKeys.IdToken"/> ("id_token").
    /// </summary>
    public string IdToken { get; set; } = BffTokenKeys.IdToken;

    /// <summary>
    /// Gets or sets the key name used to store the token expiration timestamp.
    /// Defaults to <see cref="BffTokenKeys.ExpiresAt"/> ("expires_at").
    /// </summary>
    public string ExpiresAt { get; set; } = BffTokenKeys.ExpiresAt;
}

/// <summary>
/// Represents the configuration options for the Backend-for-Frontend (BFF).
/// </summary>
public sealed class BackendForFrontendAuthenticationOptions
{
    /// <summary>
    /// The configuration section name.
    /// </summary>
    public const string SectionName = "Methrax:BffAuthentication";

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
    /// Indicates whether OpenID Connect tokens (e.g., access token, refresh token, and ID token)
    /// should be persisted in the authentication ticket.
    ///
    /// Defaults to <c>false</c> to prevent unnecessary token storage. When enabled, tokens can be
    /// accessed via <c>HttpContext.GetTokenAsync()</c>.
    /// </summary>
    public bool SaveTokens { get; set; } = false;

    /// <summary>
    /// Property key names used when storing tokens in AuthenticationProperties.
    /// </summary>
    public BffTokenOptions Tokens { get; set; } = new();
    /// <summary>
    /// When true (default), token expiration timestamps ('expires_at') are stored in UTC.
    /// When false, the server's local time zone offset is used.
    /// </summary>
    public bool UseUtcTimeZone { get; set; } = true;

    /// <summary>
    /// Enables server-side session management using an <see cref="ITicketStore"/> implementation.
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool EnableServerSideSessions { get; set; } = false;
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