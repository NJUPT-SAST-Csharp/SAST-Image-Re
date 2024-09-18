using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Authorization;
using Refit;
using WebApp.APIs.Auth;
using WebApp.Requests;

namespace WebApp.APIs;

public static class APIConfigurations
{
    public const string BaseUrl = "http://localhost:5265/api/";

    private static readonly JsonSerializerOptions options =
        new()
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };

    public static IServiceCollection AddApiClients(this IServiceCollection services)
    {
        services.AddApiClient<IAlbumAPI>("albums");
        services.AddApiClient<IAccountAPI>("account");

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthorizationCore().AddCascadingAuthenticationState();
        services.AddSingleton<AuthStateProvider>();
        services.AddSingleton<AuthenticationStateProvider>(p =>
            p.GetRequiredService<AuthStateProvider>()
        );

        return services;
    }

    private static IServiceCollection AddApiClient<T>(
        this IServiceCollection services,
        string? name = null
    )
        where T : class
    {
        services
            .AddRefitClient<T>(p =>
                new()
                {
                    ContentSerializer = new SystemTextJsonContentSerializer(options),
                    AuthorizationHeaderValueGetter = async (
                        HttpRequestMessage request,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        string? token = await p.GetRequiredService<AuthStateProvider>()
                            .GetTokenAsync();
                        return token ?? string.Empty;
                    },
                }
            )
            .ConfigureHttpClient(client => client.BaseAddress = new(BaseUrl + name));

        return services;
    }
}
