using Domain.AlbumDomain.Events;
using Domain.Core.Event;

namespace Application.AlbumServices.EventHandlers
{
    internal sealed class AlbumCoverUpdatedEventHandler(IAlbumCoverManager updater)
        : IDomainEventHandler<AlbumCoverUpdatedEvent>
    {
        private readonly IAlbumCoverManager _updater = updater;

        public Task Handle(AlbumCoverUpdatedEvent notification, CancellationToken cancellationToken)
        {
            if (notification.ContainedImage is not null)
            {
                return _updater.UpdateWithContainedImageAsync(
                    notification.Album,
                    notification.ContainedImage.Value,
                    cancellationToken
                );
            }

            if (notification.CoverImage is not null)
            {
                return _updater.UpdateWithCustomImageAsync(
                    notification.Album,
                    notification.CoverImage,
                    cancellationToken
                );
            }

            return _updater.RemoveCoverAsync(notification.Album, cancellationToken);
        }
    }
}
