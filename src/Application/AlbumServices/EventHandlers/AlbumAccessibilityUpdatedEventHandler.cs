using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Events;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.AlbumServices.EventHandlers
{
    internal sealed class AlbumAccessibilityUpdatedEventHandler(
        IRepository<AlbumModel, AlbumId> repository
    ) : IDomainEventHandler<AlbumAccessibilityUpdatedEvent>
    {
        private readonly IRepository<AlbumModel, AlbumId> _repository = repository;

        public async Task Handle(
            AlbumAccessibilityUpdatedEvent notification,
            CancellationToken cancellationToken
        )
        {
            var album = await _repository.GetAsync(notification.Album, cancellationToken);

            album.Accessibility = notification.Accessibility.Value;
        }
    }
}
