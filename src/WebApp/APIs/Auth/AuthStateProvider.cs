using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace WebApp.APIs.Auth;

public sealed class AuthStateProvider(ILocalStorageService localStorage)
    : AuthenticationStateProvider
{
    public async Task SetTokenAsync(string? token)
    {
        if (token == null)
            await localStorage.RemoveItemAsync("auth_token");
        else
            await localStorage.SetItemAsync("auth_token", token);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<string?> GetTokenAsync()
    {
        string? token = await localStorage.GetItemAsync<string>("auth_token");

        if (token is null)
            return null;

        long expiry = new JsonWebToken(token).GetPayloadValue<long>(JwtRegisteredClaimNames.Exp);
        var expTime = DateTimeOffset.FromUnixTimeSeconds(expiry).UtcDateTime;

        if (expTime > DateTime.UtcNow)
            return token;
        else
            await SetTokenAsync(null);

        return null;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? token = await GetTokenAsync();

        var identity = string.IsNullOrEmpty(token)
            ? new ClaimsIdentity()
            : new ClaimsIdentity(new JsonWebToken(token).Claims, JwtConstants.TokenType);

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
}
