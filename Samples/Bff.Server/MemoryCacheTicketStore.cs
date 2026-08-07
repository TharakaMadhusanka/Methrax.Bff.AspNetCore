using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
internal class MemoryCacheTicketStore(IMemoryCache cache) : ITicketStore
{
    private readonly IMemoryCache _cache = cache;

    public async Task<string> StoreAsync(AuthenticationTicket ticket)
    {
        var key = Guid.NewGuid().ToString("N");
        _cache.Set(key, ticket);
        Console.WriteLine($"[ITicketStore] Storing ticket in RAM with key: {key} (Length: {key.Length} chars)");
        return key;
    }

    public Task<AuthenticationTicket?> RetrieveAsync(string key)
    {
        Console.WriteLine($"[ITicketStore] Fetching ticket from RAM for key: {key}");
        _cache.TryGetValue(key, out AuthenticationTicket? ticket);
        return Task.FromResult(ticket);
    }

    public Task RenewAsync(string key, AuthenticationTicket ticket)
    {
        _cache.Set(key, ticket);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        return Task.CompletedTask;
    }
}
