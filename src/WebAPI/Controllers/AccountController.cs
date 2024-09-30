using System.ComponentModel.DataAnnotations;
using Application.Query;
using Application.UserServices.Queries;
using Domain.Command;
using Domain.UserDomain.Commands;
using Domain.UserDomain.UserEntity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities;

namespace WebAPI.Controllers;

[Route("api/account")]
[ApiController]
public sealed class AccountController(
    IDomainCommandSender commandSender,
    IQueryRequestSender querySender
) : ControllerBase
{
    private readonly IDomainCommandSender _commanderSender = commandSender;
    private readonly IQueryRequestSender _querySender = querySender;

    public sealed record RegisterRequest(
        [Length(Username.MinLength, Username.MaxLength)] string Username,
        [Length(PasswordInput.MinLength, PasswordInput.MaxLength)] string Password,
        [Range(RegistryCode.MinValue, RegistryCode.MaxValue)] int Code
    );

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken
    )
    {
        if (Username.TryCreateNew(request.Username, out var username) == false)
            return this.ValidationFail(request.Username, nameof(request.Username));
        if (PasswordInput.TryCreateNew(request.Password, out var password) == false)
            return this.ValidationFail(request.Password, nameof(request.Password));
        if (RegistryCode.TryCreateNew(request.Code, out var code) == false)
            return this.ValidationFail(request.Code, nameof(request.Code));

        RegisterCommand command = new(username, password, code);
        var result = await _commanderSender.SendAsync(command, cancellationToken);

        return Ok(new { token = result.Value });
    }

    public sealed record LoginRequest(
        [Length(Username.MinLength, Username.MaxLength)] string Username,
        [Length(PasswordInput.MinLength, PasswordInput.MaxLength)] string Password
    );

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken
    )
    {
        if (Username.TryCreateNew(request.Username, out var username) == false)
            return this.ValidationFail(request.Username, nameof(request.Username));
        if (PasswordInput.TryCreateNew(request.Password, out var password) == false)
            return this.ValidationFail(request.Password, nameof(request.Password));

        LoginCommand command = new(username, password);

        var result = await _commanderSender.SendAsync(command, cancellationToken);

        return Ok(new { token = result.Value });
    }

    public sealed record ResetPasswordRequest(
        [Length(PasswordInput.MinLength, PasswordInput.MaxLength)] string OldPassword,
        [Length(PasswordInput.MinLength, PasswordInput.MaxLength)] string NewPassword
    );

    [HttpPost("reset/password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordRequest request,
        CancellationToken cancellationToken
    )
    {
        if (PasswordInput.TryCreateNew(request.OldPassword, out var oldPassword) == false)
            return this.ValidationFail(request.OldPassword, nameof(request.OldPassword));
        if (PasswordInput.TryCreateNew(request.NewPassword, out var newPassword) == false)
            return this.ValidationFail(request.NewPassword, nameof(request.NewPassword));

        ResetPasswordCommand command = new(oldPassword, newPassword, new(User));

        await _commanderSender.SendAsync(command, cancellationToken);

        return NoContent();
    }

    public sealed record ResetUsernameRequest(
        [Length(Username.MinLength, Username.MaxLength)] string Username
    );

    [HttpPost("reset/username")]
    public async Task<IActionResult> ResetUsername(
        [FromBody] ResetUsernameRequest request,
        CancellationToken cancellationToken
    )
    {
        if (Username.TryCreateNew(request.Username, out var username) == false)
            return this.ValidationFail(request.Username, nameof(request.Username));

        ResetUsernameCommand command = new(username, new(User));

        await _commanderSender.SendAsync(command, cancellationToken);

        return NoContent();
    }

    [HttpGet("username/check")]
    public async Task<IActionResult> CheckUsernameExistence(
        [FromQuery] [Length(Username.MinLength, Username.MaxLength)] string username,
        CancellationToken cancellationToken
    )
    {
        if (Username.TryCreateNew(username, out var name) == false)
            return this.ValidationFail(username, nameof(username));

        var query = new UsernameExistenceQuery(name);
        var result = await _querySender.SendAsync(query, cancellationToken);
        return Ok(result);
    }
}
