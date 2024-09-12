using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.AlbumDomain.Events
{
    public sealed record class ImageAddedEvent(
        AlbumId Album,
        ImageId ImageId,
        UserId AuthorId,
        ImageTitle Title,
        ImageTags Tags,
        AccessLevel AccessLevel,
        Collaborators Collaborators,
        Stream ImageFile,
        DateTime CreatedAt
    ) : IDomainEvent { }
}
