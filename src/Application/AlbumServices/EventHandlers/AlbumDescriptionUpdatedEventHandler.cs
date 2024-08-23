using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Events;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.AlbumServices.EventHandlers
{
    internal sealed class AlbumDescriptionUpdatedEventHandler(
        IRepository<AlbumModel, AlbumId> repository
    ) : IDomainEventHandler<AlbumDescriptionUpdatedEvent>
    {
        private readonly IRepository<AlbumModel, AlbumId> _repository = repository;

        public async Task Handle(
            AlbumDescriptionUpdatedEvent notification,
            CancellationToken cancellationToken
        )
        {
            var album = await _repository.GetAsync(notification.Album, cancellationToken);

            album.Description = notification.Description.Value;
        }
    }
}
