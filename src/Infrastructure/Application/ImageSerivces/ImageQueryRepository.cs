using Application;
using Application.ImageServices.Queries;
using Domain.AlbumDomain.AlbumEntity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Application.ImageServices
{
    internal sealed class ImageQueryRepository(QueryDbContext context)
        : IQueryRepository<AlbumImagesQuery, List<AlbumImageDto>>,
            IQueryRepository<RemovedImagesQuery, List<RemovedImageDto>>
    {
        private readonly QueryDbContext _context = context;

        public Task<List<AlbumImageDto>> GetOrDefaultAsync(
            AlbumImagesQuery query,
            CancellationToken cancellationToken = default
        )
        {
            return _context
                .Images.AsNoTracking()
                .Where(i =>
                    i.AlbumId == query.Album.Value
                    && (
                        i.Accessibility == AccessibilityValue.Public
                        || (
                            i.Accessibility == AccessibilityValue.AuthOnly
                            && query.Actor.IsAuthenticated
                        )
                        || (
                            i.Accessibility == AccessibilityValue.Private
                            && (i.AuthorId == query.Actor.Id.Value || query.Actor.IsAdmin)
                        )
                    )
                )
                .Select(i => new AlbumImageDto(i.Id, i.Title))
                .ToListAsync(cancellationToken);
        }

        public Task<List<RemovedImageDto>> GetOrDefaultAsync(
            RemovedImagesQuery query,
            CancellationToken cancellationToken = default
        )
        {
            return _context
                .Images.IgnoreQueryFilters()
                .AsNoTracking()
                .Where(i =>
                    i.RemovedAt != null
                    && i.AlbumId == query.Album.Value
                    && (i.AuthorId == query.Actor.Id.Value || query.Actor.IsAdmin)
                )
                .Select(i => new RemovedImageDto(i.Id, i.Title))
                .ToListAsync(cancellationToken);
        }
    }
}
