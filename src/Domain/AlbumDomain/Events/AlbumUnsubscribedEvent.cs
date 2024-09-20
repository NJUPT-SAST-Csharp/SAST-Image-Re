using Domain.AlbumDomain.AlbumEntity;
using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.AlbumDomain.Events;

public sealed record class AlbumUnsubscribedEvent(AlbumId Album, UserId User) : IDomainEvent { }
