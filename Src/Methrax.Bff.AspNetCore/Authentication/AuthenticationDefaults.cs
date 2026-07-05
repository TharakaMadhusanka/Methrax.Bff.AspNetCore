using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Methrax.Bff.AspNetCore.Authentication
{
    internal static class AuthenticationDefaults
    {
        public const string CookieScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        public const string OpenIdConnectScheme = OpenIdConnectDefaults.AuthenticationScheme;
    }
}

