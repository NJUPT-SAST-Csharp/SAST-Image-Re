using Domain.AlbumDomain.AlbumEntity;
using Domain.Event;

namespace Domain.AlbumDomain.Events
{
    public readonly record struct AlbumRemovedEvent(AlbumId Album) : IDomainEvent { }
}
