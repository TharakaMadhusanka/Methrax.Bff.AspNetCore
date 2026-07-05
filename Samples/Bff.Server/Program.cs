using Methrax.Bff.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Register the YARP Reverse Proxy core engine from appsettings
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add Bff Middleware to handle authentication and authorization flows in a BFF architecture.
builder.Services.AddBffAuthentication();
// Enable Authorization to protect the backend API endpoints and ensure that only authenticated users can access them.
builder.Services.AddAuthorization();

// Add services to the container.

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Enable Reverse Proxy to forward requests to the backend API.
// This is useful in a BFF architecture where the frontend communicates with the backend through the BFF.
app.MapReverseProxy();

/**
 * Apis required for Backend for Frontend (BFF) pattern. These endpoints are used to handle authentication and authorization flows.
 * this will be used to trigger the OIDC challenge and redirect the user to the identity provider for authentication.
 * **/
app.MapGet("/login", (HttpContext context) =>
{
    var properties = new AuthenticationProperties
    {
        // Can configure the redirect URI after successful login
        // But for now I will redirect to the Angular app root path after successful login
        RedirectUri = "/"
    };

    // Triggers the OIDC challenge to redirect to the identity provider securely
    return Results.Challenge(
        properties: properties,
        authenticationSchemes: [OpenIdConnectDefaults.AuthenticationScheme]
    );
});

app.MapGet("/logout", async (HttpContext context) =>
{
    var properties = new AuthenticationProperties
    {
        RedirectUri = "/"
    };
    return Results.SignOut(properties: properties,
        authenticationSchemes: [CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme]);
});

// This endpoint is used to get the authenticated user's information. It checks if the user is authenticated and returns their name and claims.
// If the user is not authenticated, it returns a 401 Unauthorized response.
app.MapGet("/user", (HttpContext context) =>
{
    if (context.User.Identity?.IsAuthenticated ?? false)
    {
        var claims = context.User.Claims.Select(c => new { c.Type, c.Value });
        return Results.Ok(new User($"{context.User.FindFirstValue(ClaimTypes.GivenName)} {context.User.FindFirstValue(ClaimTypes.Surname)}" ?? string.Empty,
            context.User.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
            [.. context.User.FindAll(ClaimTypes.Role).Select(c => c.Value)]));
    }
    else
    {
        return Results.Unauthorized();
    }
});

app.UseHttpsRedirection();

app.Run();

sealed record User(string Name, string Email, string[] Roles);
