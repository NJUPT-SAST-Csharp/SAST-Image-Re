using Domain.AlbumDomain.Events;
using Domain.AlbumDomain.ImageEntity;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.ImageServices.EvengHandlers
{
    internal sealed class ImageRemovedEventHandler(IRepository<ImageModel, ImageId> repository)
        : IDomainEventHandler<ImageRemovedEvent>
    {
        private readonly IRepository<ImageModel, ImageId> _repository = repository;

        public async Task Handle(
            ImageRemovedEvent notification,
            CancellationToken cancellationToken
        )
        {
            var image = await _repository.GetAsync(notification.Image, cancellationToken);

            image.IsRemoved = true;
        }
    }
}
