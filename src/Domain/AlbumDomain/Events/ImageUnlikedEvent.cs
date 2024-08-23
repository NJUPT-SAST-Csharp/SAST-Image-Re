using Domain.AlbumDomain.ImageEntity;
using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.AlbumDomain.Events
{
    public readonly record struct ImageUnlikedEvent(ImageId Image, UserId User) : IDomainEvent { }
}
