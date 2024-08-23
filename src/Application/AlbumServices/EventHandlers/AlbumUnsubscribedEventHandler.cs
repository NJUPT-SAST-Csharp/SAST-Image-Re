using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Events;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.AlbumServices.EventHandlers
{
    internal sealed class AlbumUnsubscribedEventHandler(IRepository<AlbumModel, AlbumId> repository)
        : IDomainEventHandler<AlbumUnsubscribedEvent>
    {
        private readonly IRepository<AlbumModel, AlbumId> _repository = repository;

        public async Task Handle(AlbumUnsubscribedEvent e, CancellationToken cancellationToken)
        {
            var album = await _repository.GetAsync(e.Album, cancellationToken);

            album.Subscribes.RemoveAll(s => s.User == e.User.Value);
        }
    }
}
