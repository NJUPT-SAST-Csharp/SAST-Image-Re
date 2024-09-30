using Masa.Blazor;
using Microsoft.AspNetCore.Components;
using WebApp.APIs;
using WebApp.Utils;

namespace WebApp.Pages.Auth;

public sealed partial class LoginPage
{
    [SupplyParameterFromForm]
    LoginRequest Request { get; set; } = null!;

    private bool loading = false;

    protected override void OnInitialized()
    {
        Request = new(I18n);
        base.OnInitialized();
    }

    async Task Submit()
    {
        if (loading)
            return;

        loading = true;

        var response = await Api.Login(Request);

        if (response.IsSuccessStatusCode)
        {
            await Auth.SetTokenAsync(response.Content!.Token);
            Nav.NavigateTo("/user/" + State.Value.Id);
        }
        else
        {
            ExceptionRequest.Value = new ExceptionRequest
            {
                StatusCode = response.StatusCode,
                Message = I18n.T("login_failed"),
            };
        }

        loading = false;
    }
}

public sealed class LoginRequest(I18n I18n)
{
    public readonly Func<string, StringBoolean>[] UsernameValidators =
    [
        s => s.Length >= 2 && s.Length <= 16 ? true : I18n.T("username_length_invalid"),
        s => s.IsValid() ? true : I18n.T("username_characters_invalid"),
    ];
    public readonly Func<string, StringBoolean>[] PasswordValidators =
    [
        s => s.Length >= 6 && s.Length <= 20 ? true : I18n.T("password_length_invalid"),
        s => s.IsValid() ? true : I18n.T("password_characters_invalid"),
    ];

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public sealed record LoginResponse(string Token);
