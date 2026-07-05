using Microsoft.AspNetCore.Http;

namespace Methrax.Bff.AspNetCore.Options;

/// <summary>
/// Cookie configuration for the BFF authentication session.
/// </summary>
public sealed class CookieOptions
{
    /// <summary>
    /// Cookie SameSite policy.
    /// </summary>
    public SameSiteMode SameSite { get; set; } = SameSiteMode.Strict;

    /// <summary>
    /// Cookie secure policy.
    /// </summary>
    public CookieSecurePolicy SecurePolicy { get; set; } = CookieSecurePolicy.Always;

    /// <summary>
    /// Indicates whether the cookie is HTTP only.
    /// </summary>
    public bool HttpOnly { get; set; } = true;
}