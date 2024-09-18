using Domain.AlbumDomain.ImageEntity;
using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.AlbumDomain.Events;

public sealed record class ImageUnlikedEvent(ImageId Image, UserId User) : IDomainEvent { }
