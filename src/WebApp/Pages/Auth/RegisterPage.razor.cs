using Masa.Blazor;
using Microsoft.AspNetCore.Components;
using WebApp.APIs;
using WebApp.Utils;

namespace WebApp.Pages.Auth;

public sealed partial class RegisterPage
{
    [SupplyParameterFromForm]
    private RegisterRequest Request { get; set; } = null!;

    private bool loading = false;

    private string code = string.Empty;

    private bool isUsernameDisabled = false;

    private bool isUsernameValid = false;
    private bool isIconDisabled = false;
    private string iconColor = "info";
    private readonly List<string> usernameValidationMessage = [];

    private MTextField<string> _usernameTextField = null!;

    protected override void OnInitialized()
    {
        Request = new RegisterRequest(I18n);
        base.OnInitialized();
    }

    private async Task Submit()
    {
        if (isUsernameValid == false)
            return;

        loading = true;

        Request.Code = int.Parse(code);
        var response = await Api.Register(Request);

        if (response.IsSuccessStatusCode)
        {
            await Auth.SetTokenAsync(response.Content!.Token);
            Nav.NavigateTo("/user/" + State.Value.Id);
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            ExceptionRequest.Value = new ExceptionRequest
            {
                StatusCode = response.StatusCode,
                Message = I18n.T("username_exists"),
            };

            SetUsernameDuplicated_(true);
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

        loading = false;
    }

    private async void CheckUsername()
    {
        iconColor = string.Empty;

        if (string.IsNullOrWhiteSpace(Request.Username) || _usernameTextField.Validate() == false)
        {
            SetUsernameDuplicated_(null);
            return;
        }

        isUsernameDisabled = true;
        string value = Request.Username;

        var response = await Api.CheckUsernameExistence(value);
        if (response.StatusCode != System.Net.HttpStatusCode.Conflict)
            SetUsernameDuplicated_(false);
        else
            SetUsernameDuplicated_(true);

        isUsernameDisabled = false;
        StateHasChanged();
    }

    private void ClearUsernameDuplicatedError()
    {
        SetUsernameDuplicated_(null);
    }

    private void OnCodeInput(string value)
    {
        if (int.TryParse(code, out int i) == false)
            code = string.Empty;
        else
            Request.Code = i;
    }

    private void SetUsernameDuplicated_(bool? duplicated)
    {
        if (duplicated is null)
        {
            isUsernameValid = true;
            iconColor = "info";
            isIconDisabled = false;
            usernameValidationMessage.RemoveAll(s => s == I18n.T("username_exists"));
        }
        else if (duplicated.Value)
        {
            isUsernameValid = false;
            iconColor = "error";
            isIconDisabled = true;
            usernameValidationMessage.Add(I18n.T("username_exists"));
        }
        else
        {
            isUsernameValid = true;
            iconColor = "success";
            isIconDisabled = false;
            usernameValidationMessage.RemoveAll(s => s == I18n.T("username_exists"));
        }
        StateHasChanged();
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
