using Domain.Command;
using Domain.Extensions;
using Domain.Shared;
using Domain.UserDomain.UserEntity;
using Domain.UserEntity.Services;

namespace Domain.UserDomain.Commands
{
    public sealed record UpdateUsernameCommand(Username Username, Actor Actor) : IDomainCommand;

    internal sealed class UpdateUsernameCommandHandler(
        IRepository<User, UserId> repository,
        IUsernameUniquenessChecker checker
    ) : IDomainCommandHandler<UpdateUsernameCommand>
    {
        public async Task Handle(UpdateUsernameCommand command, CancellationToken cancellationToken)
        {
            await checker.CheckAsync(command.Username, cancellationToken);

            var user = await repository.GetAsync(command.Actor.Id, cancellationToken);

            user.UpdateUsername(command);
        }
    }
}
