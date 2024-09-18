using Domain.Command;
using Domain.Extensions;
using Domain.Shared;
using Domain.TagDomain.TagEntity;

namespace Domain.TagDomain.Commands;

public sealed record UpdateTagCommand(TagId Id, TagName NewName, Actor Actor) : IDomainCommand { }

internal sealed class UpdateTagCommandHandler(IRepository<Tag, TagId> repository)
    : IDomainCommandHandler<UpdateTagCommand>
{
    private readonly IRepository<Tag, TagId> _repository = repository;

    public async Task Handle(UpdateTagCommand command, CancellationToken cancellationToken)
    {
        if (command.Actor.IsAdmin == false)
            throw new NoPermissionException();

        var tag = await _repository.GetAsync(command.Id, cancellationToken);

        tag.Update(command);
    }
}
