using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Events;

public sealed record AvatarUpdatedEvent(UserId User, Stream Avatar) : IDomainEvent { }
