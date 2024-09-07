using Domain.Shared;
using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Commands
{
    public record ResetPasswordCommand(
        PasswordInput OldPassword,
        PasswordInput NewPassword,
        Actor Actor
    );
}
