using Domain.AlbumDomain.Events;
using Domain.AlbumDomain.ImageEntity;
using Domain.Core.Event;
using Domain.Extensions;
using Domain.UserDomain.UserEntity;

namespace Application.ImageServices.EvengHandlers;

internal sealed class ImageUnlikedEventHandler(IRepository<LikeModel, (ImageId, UserId)> repository)
    : IDomainEventHandler<ImageUnlikedEvent>
{
    private readonly IRepository<LikeModel, (ImageId, UserId)> _repository = repository;

    public Task Handle(ImageUnlikedEvent e, CancellationToken cancellationToken)
    {
        return _repository.DeleteAsync((e.Image, e.User), cancellationToken);
    }
}
