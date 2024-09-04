using Domain.Core.Event;
using Domain.Extensions;
using Domain.TagDomain.Events;
using Domain.TagDomain.TagEntity;

namespace Application.TagServices.EventHandlers
{
    internal sealed class TagUpdatedEventHandler(IRepository<TagModel, TagId> repository)
        : IDomainEventHandler<TagUpdatedEvent>
    {
        private readonly IRepository<TagModel, TagId> _repository = repository;

        public async Task Handle(TagUpdatedEvent e, CancellationToken cancellationToken)
        {
            var tag = await _repository.GetAsync(e.Id, cancellationToken);

            tag.Update(e);
        }
    }
}
