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
                .Albums.AsNoTracking()
                .Where(a => a.Id == albumId.Value)
                .Select(a => new
                {
                    a.AccessLevel,
                    a.AuthorId,
                    a.Status,
                    a.Collaborators,
                })
                .FirstOrDefaultAsync(cancellationToken);

            long actorId = actor.Id.Value;

            if (album is null)
                return false;
            if (actor.IsAdmin)
                return true;
            if (album.Status == AlbumStatusValue.Removed)
            {
                if (album.AuthorId == actorId)
                    return true;
                return false;
            }

            return album.AccessLevel switch
            {
                AccessLevelValue.PublicReadOnly => true,
                AccessLevelValue.AuthReadOnly => actor.IsAuthenticated,
                AccessLevelValue.Private => album.AuthorId == actorId
                    || album.Collaborators.Contains(actorId),
                _ => false,
            };
        }
    }
}
