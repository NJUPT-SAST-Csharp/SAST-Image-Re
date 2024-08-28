using Domain.AlbumDomain.Events;
using Domain.AlbumDomain.ImageEntity;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.ImageServices.EvengHandlers
{
    internal sealed class ImageAddedEventHandler(
        IRepository<ImageModel, ImageId> repository,
        IImageStorageManager manager
    ) : IDomainEventHandler<ImageAddedEvent>
    {
        private readonly IRepository<ImageModel, ImageId> _repository = repository;
        private readonly IImageStorageManager _manager = manager;

        public async Task Handle(ImageAddedEvent e, CancellationToken cancellationToken)
        {
            ImageModel image =
                new()
                {
                    Id = e.ImageId.Value,
                    AlbumId = e.Album.Value,
                    AuthorId = e.AuthorId.Value,
                    Title = e.Title.Value,
                    Tags = e.Tags.Select(tag => tag.Value).ToArray(),
                };

            await _repository.AddAsync(image, cancellationToken);

            await _manager.StoreImageAsync(e.ImageId, e.ImageFile, cancellationToken);
        }
    }
}
