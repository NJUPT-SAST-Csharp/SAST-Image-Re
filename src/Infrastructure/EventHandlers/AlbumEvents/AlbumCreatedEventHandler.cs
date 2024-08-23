using Application.AlbumServices;
using Domain.AlbumDomain.Events;
using Domain.Core.Event;
using Infrastructure.Database;

namespace Infrastructure.EventHandlers.AlbumEvents
{
    internal sealed class AlbumCreatedEventHandler(QueryDbContext context)
        : IDomainEventHandler<AlbumCreatedEvent>
    {
        private readonly QueryDbContext _context = context;

        public async Task Handle(AlbumCreatedEvent e, CancellationToken cancellationToken)
        {
            AlbumModel album =
                new()
                {
                    Id = e.AlbumId.Value,
                    Accessibility = e.Accessibility.Value,
                    AuthorId = e.AuthorId.Value,
                    CategoryId = e.CategoryId.Value,
                    Description = e.Description.Value,
                    Title = e.Title.Value,
                };

            await _context.Albums.AddAsync(album, cancellationToken);
        }
    }
}
