using Domain.Command;
using Domain.Extensions;
using Domain.Shared;
using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Commands;

public sealed record UpdateHeaderCommand(Stream Header, Actor Actor) : IDomainCommand { }

internal sealed class UpdateHeaderCommandHandler(IRepository<User, UserId> repository)
    : IDomainCommandHandler<UpdateHeaderCommand>
{
    public async Task Handle(UpdateHeaderCommand command, CancellationToken cancellationToken)
    {
        var user = await repository.GetAsync(command.Actor.Id, cancellationToken);

        user.UpdateHeader(command);
    }
}
