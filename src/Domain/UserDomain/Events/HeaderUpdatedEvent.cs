using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Events;

public sealed record HeaderUpdatedEvent(UserId User, Stream Header) : IDomainEvent { }
