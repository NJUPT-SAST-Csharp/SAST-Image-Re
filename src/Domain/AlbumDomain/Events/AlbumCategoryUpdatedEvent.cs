using Domain.AlbumDomain.AlbumEntity;
using Domain.CategoryDomain.CategoryEntity;
using Domain.Event;

namespace Domain.AlbumDomain.Events;

public sealed record class AlbumCategoryUpdatedEvent(AlbumId Album, CategoryId Category)
    : IDomainEvent { }
