using Domain.AlbumDomain.Events;
using Domain.AlbumDomain.ImageEntity;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.ImageServices.EvengHandlers;

internal sealed class ImageDeletedEventHandler(
    IRepository<ImageModel, ImageId> repository,
    IImageStorageManager manager
) : IDomainEventHandler<ImageDeletedEvent>
{
    private readonly IRepository<ImageModel, ImageId> _repository = repository;
    private readonly IImageStorageManager _manager = manager;

    public Task Handle(ImageDeletedEvent e, CancellationToken cancellationToken)
    {
        return Task.WhenAll(
            _repository.DeleteAsync(e.Image, cancellationToken),
            _manager.DeleteImageAsync(e.Image, cancellationToken)
        );
    }
}
