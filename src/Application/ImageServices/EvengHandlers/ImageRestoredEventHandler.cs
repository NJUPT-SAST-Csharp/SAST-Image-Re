using Domain.AlbumDomain.Events;
using Domain.AlbumDomain.ImageEntity;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.ImageServices.EvengHandlers;

internal sealed class ImageRestoredEventHandler(IRepository<ImageModel, ImageId> repository)
    : IDomainEventHandler<ImageRestoredEvent>
{
    private readonly IRepository<ImageModel, ImageId> _repository = repository;

    public async Task Handle(ImageRestoredEvent e, CancellationToken cancellationToken)
    {
        var image = await _repository.GetAsync(e.Image, cancellationToken);

        image.Restore(e);
    }
}
