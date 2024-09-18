using Domain.Core.Event;
using Domain.Extensions;
using Domain.TagDomain.Events;
using Domain.TagDomain.TagEntity;

namespace Application.TagServices.EventHandlers;

internal sealed class TagCreatedEventHandler(IRepository<TagModel, TagId> repository)
    : IDomainEventHandler<TagCreatedEvent>
{
    private readonly IRepository<TagModel, TagId> _repository = repository;

    public Task Handle(TagCreatedEvent e, CancellationToken cancellationToken)
    {
        TagModel tag = new(e);

        return _repository.AddAsync(tag, cancellationToken);
    }
}
