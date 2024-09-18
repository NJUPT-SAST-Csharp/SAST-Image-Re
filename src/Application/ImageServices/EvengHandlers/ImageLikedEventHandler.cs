using Domain.AlbumDomain.Events;
using Domain.AlbumDomain.ImageEntity;
using Domain.Core.Event;
using Domain.Extensions;
using Domain.UserDomain.UserEntity;

namespace Application.ImageServices.EvengHandlers;

internal sealed class ImageLikedEventHandler(IRepository<LikeModel, (ImageId, UserId)> repository)
    : IDomainEventHandler<ImageLikedEvent>
{
    private readonly IRepository<LikeModel, (ImageId, UserId)> _repository = repository;

    public Task Handle(ImageLikedEvent e, CancellationToken cancellationToken)
    {
        LikeModel like = new(e.Image.Value, e.User.Value);

        return _repository.AddAsync(like, cancellationToken);
    }
}
