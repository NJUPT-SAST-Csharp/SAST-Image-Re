using Domain.AlbumDomain.Events;
using Domain.AlbumDomain.ImageEntity;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.ImageServices.EvengHandlers
{
    internal sealed class ImageUnlikedEventHandler(IRepository<ImageModel, ImageId> repository)
        : IDomainEventHandler<ImageUnlikedEvent>
    {
        private readonly IRepository<ImageModel, ImageId> _repository = repository;

        public async Task Handle(ImageUnlikedEvent e, CancellationToken cancellationToken)
        {
            var image = await _repository.GetAsync(e.Image, cancellationToken);

            image.Likes.RemoveAll(like => like.User == e.User.Value);
        }
    }
}
