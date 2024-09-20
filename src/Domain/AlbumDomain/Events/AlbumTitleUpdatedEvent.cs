using Domain.AlbumDomain.AlbumEntity;
using Domain.Event;

namespace Domain.AlbumDomain.Events;

public sealed record class AlbumTitleUpdatedEvent(AlbumId Album, AlbumTitle Title)
    : IDomainEvent { }
