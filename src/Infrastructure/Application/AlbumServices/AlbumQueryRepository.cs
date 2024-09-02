using Application;
using Application.AlbumServices.Queries;
using Domain.AlbumDomain.AlbumEntity;
using Infrastructure.Application.Shared;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Application.AlbumServices
{
    internal sealed class AlbumQueryRepository(QueryDbContext context)
        : IQueryRepository<DetailedAlbumQuery, DetailedAlbum?>,
            IQueryRepository<AlbumsQuery, List<AlbumDto>>,
            IQueryRepository<RemovedAlbumsQuery, List<RemovedAlbumDto>>
    {
        private readonly QueryDbContext _context = context;

        public Task<DetailedAlbum?> GetOrDefaultAsync(
            DetailedAlbumQuery query,
            CancellationToken cancellationToken = default
        )
        {
            return _context
                .Albums.AsNoTracking()
                .Where(a => a.Id == query.Id)
                .Where(a => a.Status == AlbumStatusValue.Available)
                .WhereIsAccessible(query.Actor)
                .Select(a => new DetailedAlbum(a, a.Subscribes.Count))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<List<AlbumDto>> GetOrDefaultAsync(
            AlbumsQuery query,
            CancellationToken cancellationToken = default
        )
        {
            return _context
                .Albums.AsNoTracking()
                .Where(a => a.Status == AlbumStatusValue.Available)
                .Where(a => query.CategoryId == null || a.CategoryId == query.CategoryId)
                .Where(a => query.AuthorId == null || a.AuthorId == query.AuthorId)
                .Where(a => query.Title == null || EF.Functions.ILike(a.Title, $"%{query.Title}%"))
                .WhereIsAccessible(query.Actor)
                .Select(a => new AlbumDto(a))
                .ToListAsync(cancellationToken);
        }

        public Task<List<RemovedAlbumDto>> GetOrDefaultAsync(
            RemovedAlbumsQuery query,
            CancellationToken cancellationToken = default
        )
        {
            return _context
                .Albums.AsNoTracking()
                .Where(a => a.Status == AlbumStatusValue.Removed)
                .Where(a => a.AuthorId == query.Actor.Id.Value || query.Actor.IsAdmin)
                .Select(a => new RemovedAlbumDto(a))
                .ToListAsync(cancellationToken);
        }
    }
}
