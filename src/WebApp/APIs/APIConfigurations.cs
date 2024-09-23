using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Authorization;
using Refit;
using WebApp.APIs.Auth;

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


    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthorizationCore().AddCascadingAuthenticationState();
        services.AddSingleton<AuthenticatedHttpClientHandler>();
        services.AddSingleton<AuthStateProvider>();
        services.AddSingleton<AuthenticationStateProvider>(p =>
            p.GetRequiredService<AuthStateProvider>()
        );

        return services;
    }


    public static IServiceCollection AddApiClient<T>(

        this IServiceCollection services,
        string? name = null
    )
        where T : class
    {
        services
            .AddRefitClient<T>(p =>
                new() { ContentSerializer = new SystemTextJsonContentSerializer(options) }
            )
            .ConfigurePrimaryHttpMessageHandler<AuthenticatedHttpClientHandler>()
            .ConfigureHttpClient(client => client.BaseAddress = new(BaseUrl + name));

        return services;
    }
}

sealed class AuthenticatedHttpClientHandler(AuthStateProvider auth) : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        string? token = await auth.GetTokenAsync();

        if (token is not null)
            request.Headers.Add("Authorization", "Bearer " + token);

        return await base.SendAsync(request, cancellationToken);
    }
}
