using Refit;
using WebApp.Pages.Auth;

namespace WebApp.APIs;

public interface IAccountAPI
{
    public const string Base = "account";

    [Post("/login")]
    public Task<IApiResponse<LoginResponse>> Login(LoginRequest loginRequest);

    [Post("/register")]
    public Task<IApiResponse<RegisterResponse>> Register(RegisterRequest registerRequest);

    [Get("/username/check")]
    public Task<IApiResponse> CheckUsernameExistence([Query] string username);
}
