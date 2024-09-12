using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Events
{
    public sealed record UsernameUpdatedEvent(UserId UserId, Username Username) : IDomainEvent;
}
