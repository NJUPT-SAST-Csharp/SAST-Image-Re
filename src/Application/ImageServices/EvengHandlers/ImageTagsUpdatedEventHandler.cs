using Domain.AlbumDomain.Events;
using Domain.AlbumDomain.ImageEntity;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.ImageServices.EvengHandlers
{
    internal sealed class ImageTagsUpdatedEventHandler(IRepository<ImageModel, ImageId> repository)
        : IDomainEventHandler<ImageTagsUpdatedEvent>
    {
        private readonly IRepository<ImageModel, ImageId> _repository = repository;

        public async Task Handle(ImageTagsUpdatedEvent e, CancellationToken cancellationToken)
        {
            var image = await _repository.GetAsync(e.Image, cancellationToken);

            image.Tags = e.Tags.Select(tag => tag.Value).ToArray();
        }
    }
}
