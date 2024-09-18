using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Events;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.AlbumServices.EventHandlers;

internal sealed class ImageAddedEventHandler(IRepository<AlbumModel, AlbumId> repository)
    : IDomainEventHandler<ImageAddedEvent>
{
    public async Task Handle(ImageAddedEvent e, CancellationToken cancellationToken)
    {
        var album = await repository.GetAsync(e.Album, cancellationToken);
    }
}
