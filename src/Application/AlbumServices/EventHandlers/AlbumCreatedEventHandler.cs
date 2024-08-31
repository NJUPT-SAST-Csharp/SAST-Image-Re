using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Events;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.AlbumServices.EventHandlers
{
    internal sealed class AlbumCreatedEventHandler(IRepository<AlbumModel, AlbumId> repository)
        : IDomainEventHandler<AlbumCreatedEvent>
    {
        private readonly IRepository<AlbumModel, AlbumId> _repository = repository;

        public Task Handle(AlbumCreatedEvent e, CancellationToken cancellationToken)
        {
            AlbumModel album = new(e);

            return _repository.AddAsync(album, cancellationToken);
        }
    }
}
