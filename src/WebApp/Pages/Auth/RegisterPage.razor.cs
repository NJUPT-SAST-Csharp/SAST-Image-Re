using Masa.Blazor;
using Microsoft.AspNetCore.Components;
using WebApp.APIs;
using WebApp.Utils;

namespace WebApp.Pages.Auth;

public sealed partial class RegisterPage
{
    [SupplyParameterFromForm]
    private RegisterRequest Request { get; set; } = null!;

    private readonly bool loading = false;

    private string code = string.Empty;

    private bool isValid = false;
    private readonly List<string> usernameValidationMessage = [];
    private MForm _form = null!;

    protected override void OnInitialized()
    {
        Request = new RegisterRequest(I18n);
        base.OnInitialized();
    }

    private async Task Submit()
    {
        if (isValid == false)
            return;

        Request.Code = int.Parse(code);
        var response = await Api.Register(Request);
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
                Message = I18n.T("register_failed"),
            };

            code = string.Empty;
        }
    }

    private async void CheckUsername(string value)
    {
        var response = await Api.CheckUsernameExistence(value);
        if (response.IsSuccessStatusCode && !response.Content.IsExist)
        {
            usernameValidationMessage.Remove(I18n.T("username_exists"));
            isValid = _form.Validate();
            return;
        }
        else
        {
            isValid = false;
            usernameValidationMessage.Add(I18n.T("username_exists"));
        }
        StateHasChanged();
    }

    private void OnInput(string value)
    {
        if (int.TryParse(code, out int i) == false)
            code = string.Empty;
        else
            Request.Code = i;
    }
}

public sealed class RegisterRequest(I18n I18n)
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
    public readonly Func<string, StringBoolean>[] CodeValidators =
    [
        s => s.Length == 6 && s.IsDigit() ? true : I18n.T("registration_code_invalid"),
    ];

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int Code { get; set; }
}

public readonly record struct RegisterResponse(string Token);

public readonly record struct CheckUsernameExistenceResponse(bool IsExist);
