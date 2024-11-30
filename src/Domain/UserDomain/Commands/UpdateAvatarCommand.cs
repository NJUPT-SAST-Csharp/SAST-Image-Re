using Domain.Command;
using Domain.Extensions;
using Domain.Shared;
using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Commands;

public sealed record class UpdateAvatarCommand(Stream Avatar, Actor Actor) : IDomainCommand { }

internal sealed class UpdateAvatarCommandHandler(IRepository<User, UserId> repository)
    : IDomainCommandHandler<UpdateAvatarCommand>
{
    public async Task Handle(UpdateAvatarCommand command, CancellationToken cancellationToken)
    {
        var user = await repository.GetAsync(command.Actor.Id, cancellationToken);

        user.UpdateAvatar(command);
    }
}
