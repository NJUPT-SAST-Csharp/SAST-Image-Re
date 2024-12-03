using Application;
using Application.ImageServices.Queries;
using Domain.AlbumDomain.ImageEntity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImageServices.Application;

internal sealed class ImageQueryRepository(QueryDbContext context)
    : IQueryRepository<RemovedImagesQuery, ImageDto[]>,
        IQueryRepository<DetailedImageQuery, DetailedImage?>,
        IQueryRepository<ImagesQuery, ImageDto[]>
{
    private readonly QueryDbContext _context = context;

    const int PageSize = 50;

    public Task<ImageDto[]> GetOrDefaultAsync(
        RemovedImagesQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return _context
            .Images.AsNoTracking()
            .Where(i => i.Status == ImageStatusValue.Removed)
            .Where(i => i.AlbumId == query.Album.Value)
            .Where(i =>
                i.AuthorId == query.Actor.Id.Value
                || i.Collaborators.Contains(query.Actor.Id.Value)
                || query.Actor.IsAdmin
            )
            .Select(i => new ImageDto(
                i.Id,
                i.UploaderId,
                i.AlbumId,
                i.Title,
                i.Tags,
                i.UploadedAt,
                i.RemovedAt
            ))
            .ToArrayAsync(cancellationToken);
    }

    public Task<DetailedImage?> GetOrDefaultAsync(
        DetailedImageQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return _context
            .Images.AsNoTracking()
            .Where(i => i.Status == ImageStatusValue.Available)
            .Where(i => i.Id == query.Image.Value)
            .WhereIsAccessible(query.Actor)
            .Select(i => new DetailedImage(
                i,
                i.Likes.Count,
                i.Likes.Select(l => l.User).Contains(query.Actor.Id.Value)
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<ImageDto[]> GetOrDefaultAsync(
        ImagesQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return _context
            .Images.AsNoTracking()
            .Where(i => i.Status == ImageStatusValue.Available)
            .Where(i => query.AuthorId == null || i.UploaderId == query.AuthorId)
            .Where(i => query.AlbumId == null || i.AlbumId == query.AlbumId)
            .WhereIsAccessible(query.Actor)
            .OrderByDescending(i => i.UploadedAt)
            .Skip(query.Page * PageSize)
            .Take(PageSize)
            .Select(i => new ImageDto(
                i.Id,
                i.UploaderId,
                i.AlbumId,
                i.Title,
                i.Tags,
                i.UploadedAt,
                i.RemovedAt
            ))
            .ToArrayAsync(cancellationToken);
    }
}
