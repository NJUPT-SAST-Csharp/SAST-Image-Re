using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Events
{
    public sealed record UserRegisteredEvent(UserId Id, Username Username) : IDomainEvent;
}
