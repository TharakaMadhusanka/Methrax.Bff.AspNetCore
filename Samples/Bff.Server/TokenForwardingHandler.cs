using Methrax.Bff.AspNetCore.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

public class TokenForwardingHandler(IHttpContextAccessor httpContextAccessor, IOptions<BackendForFrontendAuthenticationOptions> bffOptions) : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IOptions<BackendForFrontendAuthenticationOptions> _bffOptions = bffOptions;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null && context.User?.Identity?.IsAuthenticated == true)
        {
            var tokenKeys = _bffOptions.Value.Tokens;
            var accessToken = await context.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, tokenKeys.AccessToken);

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                if (!request.Headers.Contains("Authorization"))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                }
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
