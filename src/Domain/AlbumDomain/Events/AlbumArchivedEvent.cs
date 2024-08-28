using Domain.AlbumDomain.AlbumEntity;
using Domain.Event;

namespace Domain.AlbumDomain.Events
{
    public sealed record class AlbumArchivedEvent(AlbumId Album) : IDomainEvent { }
}
