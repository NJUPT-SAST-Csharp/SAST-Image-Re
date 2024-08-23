using Domain.AlbumDomain.AlbumEntity;
using Domain.CategoryDomain.CategoryEntity;
using Domain.Event;

namespace Domain.AlbumDomain.Events
{
    public readonly record struct AlbumCategoryUpdatedEvent(AlbumId Album, CategoryId Category)
        : IDomainEvent { }
}
