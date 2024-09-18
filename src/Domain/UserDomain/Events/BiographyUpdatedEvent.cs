using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Events;

public sealed record BiographyUpdatedEvent(UserId User, Biography Biography) : IDomainEvent { }
