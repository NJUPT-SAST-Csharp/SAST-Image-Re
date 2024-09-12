using Domain.Command;
using Domain.Extensions;
using Domain.Shared;
using Domain.UserDomain.Services;
using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Commands
{
    public record ResetPasswordCommand(
        PasswordInput OldPassword,
        PasswordInput NewPassword,
        Actor Actor
    ) : IDomainCommand;

    internal sealed class ResetPasswordCommandHandler(
        IRepository<User, UserId> repository,
        IPasswordValidator validator,
        IPasswordGenerator generator
    ) : IDomainCommandHandler<ResetPasswordCommand>
    {
        public async Task Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await repository.GetAsync(command.Actor.Id, cancellationToken);

            await user.ResetPasswordAsync(command, validator, generator, cancellationToken);
        }
    }
}
