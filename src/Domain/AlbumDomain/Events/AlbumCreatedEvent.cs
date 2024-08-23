using Domain.AlbumDomain.AlbumEntity;
using Domain.CategoryDomain.CategoryEntity;
using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.AlbumDomain.Events
{
    public readonly record struct AlbumCreatedEvent(
        AlbumId AlbumId,
        UserId AuthorId,
        CategoryId CategoryId,
        AlbumTitle Title,
        AlbumDescription Description,
        Accessibility Accessibility
    ) : IDomainEvent { }
}
