using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using WebApp.Storages;

namespace WebApp.APIs.Auth;

public sealed class AuthStateProvider(
    ILocalStorageService localStorage,
    IStatusStorage<AuthState> auth
) : AuthenticationStateProvider
{
    public async Task SetTokenAsync(string? token)
    {
        if (token == null)
        {
            await localStorage.RemoveItemAsync("auth_token");
            auth.Value = new();
        }
        else
        {
            await localStorage.SetItemAsync("auth_token", token);
            var jwt = new JsonWebToken(token);
            long id = jwt.GetPayloadValue<long>("id");
            string username = jwt.GetPayloadValue<string>("username");

            auth.Value = new(id, username);
        }

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

public readonly record struct AuthState(long Id, string Username);
