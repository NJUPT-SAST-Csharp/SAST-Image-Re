using Domain.Core.Event;
using Domain.UserDomain.Events;

namespace Application.UserServices.EventHandlers;

internal sealed class HeaderUpdatedEventHandler(IHeaderStorageManager manager)
    : IDomainEventHandler<HeaderUpdatedEvent>
{
    public Task Handle(HeaderUpdatedEvent e, CancellationToken cancellationToken)
    {
        return manager.UpdateAsync(e.User, e.Header, cancellationToken);
    }
}
