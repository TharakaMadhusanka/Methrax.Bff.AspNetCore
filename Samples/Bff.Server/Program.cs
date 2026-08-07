using Methrax.Bff.AspNetCore;
using Methrax.Bff.AspNetCore.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------------
// 1. Service Registrations
// -----------------------------------------------------------------------------

// Infrastructure required when EnableServerSideSessions is true
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ITicketStore, MemoryCacheTicketStore>();

// YARP Reverse Proxy
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// BFF Authentication extension
builder.Services.AddBffAuthentication(options =>
{
    options.EnableServerSideSessions = true;
    options.SaveTokens = true;
});

var app = builder.Build();

// -----------------------------------------------------------------------------
// 2. Middleware Pipeline
// -----------------------------------------------------------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

// Forward requests to backend services via YARP
app.MapReverseProxy();

// -----------------------------------------------------------------------------
// 3. BFF Authentication Endpoints
// -----------------------------------------------------------------------------

// GET /login - Triggers OIDC challenge redirect
app.MapGet("/login", () =>
{
    var properties = new AuthenticationProperties
    {
        RedirectUri = "/"
    };

    return Results.Challenge(
        properties: properties,
        authenticationSchemes: [OpenIdConnectDefaults.AuthenticationScheme]
    );
});

// GET /logout - Clears local cookie and initiates OIDC RP-Initiated Sign-Out
app.MapGet("/logout", () =>
{
    var properties = new AuthenticationProperties
    {
        RedirectUri = "/"
    };

    return Results.SignOut(
        properties: properties,
        authenticationSchemes:
        [
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIdConnectDefaults.AuthenticationScheme
        ]
    );
});

// GET /user - Returns user profile claims for the frontend shell
app.MapGet("/user", (HttpContext context) =>
{
    if (context.User.Identity?.IsAuthenticated != true)
    {
        return Results.Unauthorized();
    }

    var givenName = context.User.FindFirstValue(ClaimTypes.GivenName);
    var surname = context.User.FindFirstValue(ClaimTypes.Surname);
    var email = context.User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
    var roles = context.User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToArray();

    var fullName = string.IsNullOrWhiteSpace($"{givenName} {surname}")
        ? context.User.Identity.Name ?? string.Empty
        : $"{givenName} {surname}".Trim();

    return Results.Ok(new UserDto(fullName, email, roles));
});

// GET /token-info - Returns session token metadata for diagnostic inspection and debugging.
// Supports token delegation scenarios where the BFF retrieves current session tokens 
// and forwards them as Bearer tokens to downstream microservice APIs for authorization.
app.MapGet("/token-info", async (
    HttpContext context,
    IOptions<BackendForFrontendAuthenticationOptions> bffOptions) =>
{
    var tokenKeys = bffOptions.Value.Tokens;

    var accessToken = await context.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, tokenKeys.AccessToken);
    var refreshToken = await context.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, tokenKeys.RefreshToken);
    var idToken = await context.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, tokenKeys.IdToken);
    var expiresAt = await context.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, tokenKeys.ExpiresAt);

    if (string.IsNullOrEmpty(accessToken))
    {
        return Results.Ok(new
        {
            Message = "Tokens are not persisted in session (SaveTokens is disabled).",
            IsAuthenticated = true,
            UserClaims = context.User.Claims.Select(c => new { c.Type, c.Value })
        });
    }

    return Results.Ok(new
    {
        AccessToken = accessToken,
        RefreshToken = refreshToken,
        IdToken = idToken,
        ExpiresAt = expiresAt
    });
}).RequireAuthorization();

app.Run();

// -----------------------------------------------------------------------------
// Data Contracts
// -----------------------------------------------------------------------------

public sealed record UserDto(string Name, string Email, string[] Roles);