using Domain.AlbumDomain.ImageEntity;
using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.AlbumDomain.Events
{
    public readonly record struct ImageLikedEvent(ImageId Image, UserId User) : IDomainEvent { }
}
