using Domain.Core.Event;
using Domain.Extensions;
using Domain.UserDomain.Events;
using Domain.UserDomain.UserEntity;

namespace Application.UserServices.EventHandlers
{
    internal sealed class UserRegisteredEventHandler(IRepository<UserModel, UserId> repository)
        : IDomainEventHandler<UserRegisteredEvent>
    {
        public Task Handle(UserRegisteredEvent e, CancellationToken cancellationToken)
        {
            UserModel user = new(e);

            return repository.AddAsync(user, cancellationToken);
        }
    }
}
