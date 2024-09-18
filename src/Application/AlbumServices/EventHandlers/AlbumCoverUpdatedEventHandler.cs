using Domain.AlbumDomain.Events;
using Domain.Core.Event;

namespace Application.AlbumServices.EventHandlers;

internal sealed class AlbumCoverUpdatedEventHandler(ICoverStorageManager updater)
    : IDomainEventHandler<AlbumCoverUpdatedEvent>
{
    private readonly ICoverStorageManager _updater = updater;

    public Task Handle(AlbumCoverUpdatedEvent e, CancellationToken cancellationToken)
    {
        if (e.ContainedImage is not null)
        {
            return _updater.UpdateWithContainedImageAsync(
                e.Album,
                e.ContainedImage.Value,
                cancellationToken
            );
        }

        if (e.CoverImage is not null)
        {
            return _updater.UpdateWithCustomImageAsync(e.Album, e.CoverImage, cancellationToken);
        }

        return _updater.DeleteCoverAsync(e.Album, cancellationToken);
    }
}
