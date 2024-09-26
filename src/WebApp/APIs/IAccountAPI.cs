using System.ComponentModel.DataAnnotations;
using Refit;

namespace WebApp.APIs;

public interface IAccountAPI
{
    public const string Base = "account";

    [Post("/login")]
    public Task<IApiResponse<LoginResponse>> Login(LoginRequest loginRequest);
}

public sealed class LoginRequest
{
    public const int UsernameMinLength = 2;
    public const int UsernameMaxLength = 16;
    public const int PasswordMinLength = 6;
    public const int PasswordMaxLength = 20;

    [Length(UsernameMinLength, UsernameMaxLength)]
    public string Username { get; set; } = string.Empty;

    [Length(PasswordMinLength, PasswordMaxLength)]
    public string Password { get; set; } = string.Empty;
}

public sealed record LoginResponse(string Token);
