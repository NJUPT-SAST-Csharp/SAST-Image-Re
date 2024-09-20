using Domain.Core.Event;
using Domain.Extensions;
using Domain.UserDomain.Events;
using Domain.UserDomain.UserEntity;

namespace Application.UserServices.EventHandlers;

internal sealed class UsernameUpdatedEventHandler(IRepository<UserModel, UserId> repository)
    : IDomainEventHandler<UsernameResetEvent>
{
    public async Task Handle(UsernameResetEvent e, CancellationToken cancellationToken)
    {
        var user = await repository.GetAsync(e.UserId, cancellationToken);

        user.ResetUsername(e);
    }
}
