using Application;
using Application.ImageServices.Queries;
using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
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
                    i.Status == ImageStatusValue.Available
                    && i.AlbumId == query.Album.Value
                    && (
                        i.AccessLevel == AccessLevelValue.PublicReadOnly
                        || i.AccessLevel == AccessLevelValue.AuthReadOnly
                            && query.Actor.IsAuthenticated
                        || i.AccessLevel == AccessLevelValue.Private
                            && (i.AuthorId == query.Actor.Id.Value || query.Actor.IsAdmin)
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
                .Images.AsNoTracking()
                .Where(i =>
                    i.Status == ImageStatusValue.Removed
                    && i.AlbumId == query.Album.Value
                    && (i.AuthorId == query.Actor.Id.Value || query.Actor.IsAdmin)
                )
                .Select(i => new RemovedImageDto(i.Id, i.Title))
                .ToListAsync(cancellationToken);
        }
    }
}
