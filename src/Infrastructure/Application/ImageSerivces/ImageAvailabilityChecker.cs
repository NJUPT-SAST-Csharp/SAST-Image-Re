using Application.ImageServices;
using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Domain.Shared;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Application.ImageSerivces
{
    internal sealed class ImageAvailabilityChecker(QueryDbContext context)
        : IImageAvailabilityChecker
    {
        private readonly QueryDbContext _context = context;

        public async Task<bool> CheckAsync(
            ImageId id,
            Actor actor,
            CancellationToken cancellationToken = default
        )
        {
            var image = await _context
                .Images.AsNoTracking()
                .Select(i => new
                {
                    i.Id,
                    i.AuthorId,
                    i.Status,
                    i.AccessLevel,
                })
                .Where(i => i.Id == id.Value)
                .FirstOrDefaultAsync(cancellationToken);

            if (image is null)
                return false;
            if (image.AuthorId == actor.Id.Value || actor.IsAdmin)
                return true;
            if (image.Status != ImageStatusValue.Available)
                return false;

            return image.AccessLevel switch
            {
                AccessLevelValue.Public => true,
                AccessLevelValue.AuthOnly => actor.IsAuthenticated,
                AccessLevelValue.Private => await _context
                    .Albums.IgnoreQueryFilters()
                    .AsNoTracking()
                    .Where(a => a.Images.Select(i => i.Id).Contains(id.Value))
                    .AnyAsync(a => a.Collaborators.Contains(actor.Id.Value), cancellationToken),

                _ => throw new InvalidOperationException("Unknown access level value."),
            };
        }
    }
}
