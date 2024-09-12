using System.ComponentModel.DataAnnotations;
using Application.Query;
using Domain.Command;
using Domain.UserDomain.Commands;
using Domain.UserDomain.UserEntity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities;

namespace WebAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public sealed class AccountController(
        IDomainCommandSender commandSender,
        IQueryRequestSender querySender
    ) : ControllerBase
    {
        private readonly IDomainCommandSender _commanderSender = commandSender;
        private readonly IQueryRequestSender _querySender = querySender;

        public record LoginRequest(
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

            var jwt = await _commanderSender.SendAsync(command, cancellationToken);

            return Ok(jwt);
        }

        public record ResetPasswordRequest(
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
    }
}
