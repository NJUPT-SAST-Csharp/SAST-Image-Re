using Application;
using Application.AlbumServices.Queries;
using Domain.AlbumDomain.AlbumEntity;
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
                .Where(a =>
                    a.Id == query.Id
                    && (a.Status == AlbumStatusValue.Available)
                    && (
                        a.AccessLevel == AccessLevelValue.Public
                        || (
                            a.AccessLevel == AccessLevelValue.AuthOnly
                            && query.Actor.IsAuthenticated
                        )
                        || (
                            a.AccessLevel == AccessLevelValue.Private
                            && (
                                a.AuthorId == query.Actor.Id.Value
                                || a.Collaborators.Contains(query.Actor.Id.Value)
                                || query.Actor.IsAdmin
                            )
                        )
                    )
                )
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
                .Where(a =>
                    (query.CategoryId == null || a.CategoryId == query.CategoryId)
                    && (a.Status == AlbumStatusValue.Available)
                    && (query.AuthorId == null || a.AuthorId == query.AuthorId)
                    && (query.Title == null || EF.Functions.ILike(a.Title, $"%{query.Title}%"))
                    && (
                        a.AccessLevel == AccessLevelValue.Public
                        || (
                            a.AccessLevel == AccessLevelValue.AuthOnly
                            && query.Actor.IsAuthenticated
                        )
                        || (
                            a.AccessLevel == AccessLevelValue.Private
                            && (
                                a.AuthorId == query.Actor.Id.Value
                                || a.Collaborators.Contains(query.Actor.Id.Value)
                                || query.Actor.IsAdmin
                            )
                        )
                    )
                )
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
                .Where(a =>
                    (a.Status == AlbumStatusValue.Removed)
                    && (a.AuthorId == query.Actor.Id.Value || query.Actor.IsAdmin)
                )
                .Select(a => new RemovedAlbumDto(a))
                .ToListAsync(cancellationToken);
        }
    }
}
