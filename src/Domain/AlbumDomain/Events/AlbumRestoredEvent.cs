using Domain.AlbumDomain.AlbumEntity;
using Domain.Event;

namespace Domain.AlbumDomain.Events
{
    public readonly record struct AlbumRestoredEvent(AlbumId Album) : IDomainEvent { }
}
