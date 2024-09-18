using Domain.Command;
using Domain.Extensions;
using Domain.Shared;
using Domain.UserDomain.UserEntity;
using Domain.UserEntity.Services;

namespace Domain.UserDomain.Commands;

public sealed record ResetUsernameCommand(Username Username, Actor Actor) : IDomainCommand;

internal sealed class ResetUsernameCommandHandler(
    IRepository<User, UserId> repository,
    IUsernameUniquenessChecker checker
) : IDomainCommandHandler<ResetUsernameCommand>
{
    public async Task Handle(ResetUsernameCommand command, CancellationToken cancellationToken)
    {
        await checker.CheckAsync(command.Username, cancellationToken);

        var user = await repository.GetAsync(command.Actor.Id, cancellationToken);

        user.ResetUsername(command);
    }
}
