using Domain.AlbumDomain.ImageEntity;
using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.AlbumDomain.Events
{
    public sealed record class ImageLikedEvent(ImageId Image, UserId User) : IDomainEvent { }
}
