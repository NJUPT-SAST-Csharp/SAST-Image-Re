using Domain.Event;
using MediatR;

namespace Domain.Core.Event
{
    public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent { }
}
