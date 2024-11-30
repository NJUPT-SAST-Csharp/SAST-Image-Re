using Domain.Core.Event;
using Domain.UserDomain.Events;

namespace Application.UserServices.EventHandlers;

internal sealed class AvatarUpdatedEventHandler(IAvatarStorageManager manager)
    : IDomainEventHandler<AvatarUpdatedEvent>
{
    public Task Handle(AvatarUpdatedEvent e, CancellationToken cancellationToken)
    {
        return manager.UpdateAsync(e.User, e.Avatar, cancellationToken);
    }
}
