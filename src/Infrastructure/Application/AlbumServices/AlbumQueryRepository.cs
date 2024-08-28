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
                    && (
                        a.Accessibility == AccessibilityValue.Public
                        || (
                            a.Accessibility == AccessibilityValue.AuthOnly
                            && query.Actor.IsAuthenticated
                        )
                        || (
                            a.Accessibility == AccessibilityValue.Private
                            && (
                                a.AuthorId == query.Actor.Id.Value
                                || a.Collaborators.Contains(query.Actor.Id.Value)
                                || query.Actor.IsAdmin
                            )
                        )
                    )
                )
                .Select(a => new DetailedAlbum(
                    a.Id,
                    a.Title,
                    a.Description,
                    a.AuthorId,
                    a.CategoryId,
                    a.IsArchived,
                    a.UpdatedAt,
                    a.CreatedAt,
                    a.Accessibility,
                    a.Subscribes.Count
                ))
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
                    && (query.AuthorId == null || a.AuthorId == query.AuthorId)
                    && (query.Title == null || EF.Functions.ILike(a.Title, $"%{query.Title}%"))
                    && (
                        a.Accessibility == AccessibilityValue.Public
                        || (
                            a.Accessibility == AccessibilityValue.AuthOnly
                            && query.Actor.IsAuthenticated
                        )
                        || (
                            a.Accessibility == AccessibilityValue.Private
                            && (
                                a.AuthorId == query.Actor.Id.Value
                                || a.Collaborators.Contains(query.Actor.Id.Value)
                                || query.Actor.IsAdmin
                            )
                        )
                    )
                )
                .Select(a => new AlbumDto(
                    a.Id,
                    a.Title,
                    a.AuthorId,
                    a.CategoryId,
                    a.IsArchived,
                    a.UpdatedAt
                ))
                .ToListAsync(cancellationToken);
        }

        public Task<List<RemovedAlbumDto>> GetOrDefaultAsync(
            RemovedAlbumsQuery query,
            CancellationToken cancellationToken = default
        )
        {
            return _context
                .Albums.IgnoreQueryFilters()
                .AsNoTracking()
                .Where(a =>
                    a.RemovedAt != null
                    && (a.AuthorId == query.Actor.Id.Value || query.Actor.IsAdmin)
                )
                .Select(a => new RemovedAlbumDto(
                    a.Id,
                    a.Title,
                    a.CategoryId,
                    a.Accessibility,
                    a.RemovedAt!.Value
                ))
                .ToListAsync(cancellationToken);
        }
    }
}
