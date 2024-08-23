using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Events;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.AlbumServices.EventHandlers
{
    internal sealed class AlbumRestoredEventHandler(IRepository<AlbumModel, AlbumId> repository)
        : IDomainEventHandler<AlbumRestoredEvent>
    {
        private readonly IRepository<AlbumModel, AlbumId> _repository = repository;

        public async Task Handle(AlbumRestoredEvent e, CancellationToken cancellationToken)
        {
            var album = await _repository.GetAsync(e.Album, cancellationToken);

            album.IsRemoved = false;
        }
    }
}
