using Application.AlbumServices;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Shared;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Application.AlbumServices
{
    internal sealed class AlbumAvailabilityChecker(QueryDbContext context)
        : IAlbumAvailabilityChecker
    {
        private readonly QueryDbContext _context = context;

        public async Task<bool> CheckAsync(
            AlbumId albumId,
            Actor actor,
            CancellationToken cancellationToken
        )
        {
            var album = await _context
                .Albums.IgnoreQueryFilters()
                .AsNoTracking()
                .Where(a => a.Id == albumId.Value)
                .Select(a => new
                {
                    a.Accessibility,
                    a.AuthorId,
                    a.RemovedAt,
                    a.Collaborators,
                })
                .FirstOrDefaultAsync(cancellationToken);

            long actorId = actor.Id.Value;

            if (album is null)
                return false;
            if (actor.IsAdmin)
                return true;
            if (album.RemovedAt is not null)
            {
                if (album.AuthorId == actorId)
                    return true;
                return false;
            }

            return album.Accessibility switch
            {
                AccessibilityValue.Public => true,
                AccessibilityValue.AuthOnly => actor.IsAuthenticated,
                AccessibilityValue.Private => album.AuthorId == actorId
                    || album.Collaborators.Contains(actorId),
                _ => false,
            };
        }
    }
}
