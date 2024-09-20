using Domain.AlbumDomain.AlbumEntity;
using Domain.Event;

namespace Domain.AlbumDomain.Events;

public sealed record class AlbumAccessLevelUpdatedEvent(AlbumId Album, AccessLevel AccessLevel)
    : IDomainEvent { }
