using Domain.Command;
using Domain.Extensions;
using Domain.UserDomain.Services;
using Domain.UserDomain.UserEntity;
using Domain.UserEntity.Services;

namespace Domain.UserDomain.Commands
{
    public sealed record RegisterCommand(
        Username Username,
        PasswordInput Password,
        Biography Biography
    ) : IDomainCommand<RegisterCommandResult>;

    public sealed record RegisterCommandResult(long Id, string Username);

    internal sealed class RegisterCommandHandler(
        IUsernameUniquenessChecker checker,
        IPasswordGenerator generator,
        IRepository<User, UserId> repository
    ) : IDomainCommandHandler<RegisterCommand, RegisterCommandResult>
    {
        public async Task<RegisterCommandResult> Handle(
            RegisterCommand command,
            CancellationToken cancellationToken
        )
        {
            await checker.CheckAsync(command.Username, cancellationToken);

            var user = await User.RegisterAsync(command, generator, cancellationToken);

            await repository.AddAsync(user, cancellationToken);

            return new(user.Id.Value, command.Username.Value);
        }
    }
}
