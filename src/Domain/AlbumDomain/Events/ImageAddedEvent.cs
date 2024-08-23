using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.AlbumDomain.Events
{
    public readonly record struct ImageAddedEvent(
        AlbumId Album,
        ImageId ImageId,
        UserId AuthorId,
        ImageTitle Title,
        ImageTags Tags,
        Stream ImageFile
    ) : IDomainEvent { }
}
