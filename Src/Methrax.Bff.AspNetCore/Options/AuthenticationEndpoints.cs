using Microsoft.AspNetCore.Http;

namespace Methrax.Bff.AspNetCore.Options;

/// <summary>
/// Authentication endpoint paths.
/// </summary>
public sealed class AuthenticationEndpoints
{
    /// <summary>
    /// Login endpoint.
    /// </summary>
    public PathString LoginPath { get; set; } = "/login";

    /// <summary>
    /// Logout endpoint.
    /// </summary>
    public PathString LogoutPath { get; set; } = "/logout";

    /// <summary>
    /// Access denied endpoint.
    /// </summary>
    public PathString AccessDeniedPath { get; set; } = "/access-denied";
}