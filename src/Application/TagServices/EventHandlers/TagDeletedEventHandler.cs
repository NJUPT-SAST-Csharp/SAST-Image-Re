using Domain.Core.Event;
using Domain.Extensions;
using Domain.TagDomain.Events;
using Domain.TagDomain.TagEntity;

namespace Application.TagServices.EventHandlers
{
    internal sealed class TagDeletedEventHandler(IRepository<TagModel, TagId> repository)
        : IDomainEventHandler<TagDeletedEvent>
    {
        private readonly IRepository<TagModel, TagId> _repository = repository;

        public Task Handle(TagDeletedEvent e, CancellationToken cancellationToken)
        {
            return _repository.DeleteAsync(e.Id, cancellationToken);
        }
    }
}
