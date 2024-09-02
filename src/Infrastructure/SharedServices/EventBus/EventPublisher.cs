using Domain.Core.Event;
using Domain.Event;
using MediatR;

namespace Infrastructure.SharedServices.EventBus
{
    internal sealed class EventPublisher(IMediator publisher) : IDomainEventPublisher
    {
        private readonly IMediator _publisher = publisher;

        public Task PublishAsync<TEvent>(
            TEvent domainEvent,
            CancellationToken cancellationToken = default
        )
            where TEvent : IDomainEvent
        {
            return _publisher.Publish(domainEvent, cancellationToken);
        }
    }
}
