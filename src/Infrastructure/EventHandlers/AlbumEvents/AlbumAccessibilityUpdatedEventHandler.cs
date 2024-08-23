using Domain.AlbumDomain.Events;
using Domain.Core.Event;
using Infrastructure.Database;

namespace Infrastructure.EventHandlers.AlbumEvents
{
    internal sealed class AlbumAccessibilityUpdatedEventHandler(QueryDbContext context)
        : IDomainEventHandler<AlbumAccessibilityUpdatedEvent>
    {
        private readonly QueryDbContext _context = context;

        public async Task Handle(
            AlbumAccessibilityUpdatedEvent e,
            CancellationToken cancellationToken
        ) { }
    }
}
