using Domain.AlbumDomain.Events;
using Domain.AlbumDomain.ImageEntity;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.ImageServices.EvengHandlers
{
    internal sealed class ImageLikedEventHandler(IRepository<ImageModel, ImageId> repository)
        : IDomainEventHandler<ImageLikedEvent>
    {
        private readonly IRepository<ImageModel, ImageId> _repository = repository;

        public async Task Handle(ImageLikedEvent e, CancellationToken cancellationToken)
        {
            var image = await _repository.GetAsync(e.Image, cancellationToken);

            image.Likes.Add(new(e.Image.Value, e.User.Value));
        }
    }
}
