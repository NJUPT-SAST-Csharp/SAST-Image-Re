using Domain.AlbumDomain.AlbumEntity;
using Domain.CategoryDomain.CategoryEntity;
using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.AlbumDomain.Events;

public sealed record AlbumCreatedEvent(
    AlbumId AlbumId,
    UserId AuthorId,
    CategoryId CategoryId,
    AlbumTitle Title,
    AlbumDescription Description,
    AccessLevel AccessLevel
) : IDomainEvent { }
