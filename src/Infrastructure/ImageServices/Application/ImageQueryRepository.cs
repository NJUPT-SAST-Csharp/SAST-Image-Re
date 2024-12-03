using Application;
using Application.ImageServices.Queries;
using Domain.AlbumDomain.ImageEntity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImageServices.Application;

internal sealed class ImageQueryRepository(QueryDbContext context)
    : IQueryRepository<AlbumImagesQuery, List<ImageDto>>,
        IQueryRepository<RemovedImagesQuery, List<ImageDto>>,
        IQueryRepository<UserImagesQuery, List<ImageDto>>,
        IQueryRepository<DetailedImageQuery, DetailedImage?>
{
    private readonly QueryDbContext _context = context;

    public Task<List<ImageDto>> GetOrDefaultAsync(
        AlbumImagesQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return _context
            .Images.AsNoTracking()
            .Where(i => i.Status == ImageStatusValue.Available)
            .Where(i => i.AlbumId == query.Album.Value)
            .WhereIsAccessible(query.Actor)
            .Select(i => new ImageDto(
                i.Id,
                i.UploaderId,
                i.AlbumId,
                i.Title,
                i.Tags,
                i.UploadedAt,
                i.RemovedAt
            ))
            .ToListAsync(cancellationToken);
    }

    public Task<List<ImageDto>> GetOrDefaultAsync(
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
            .ToListAsync(cancellationToken);
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

    public Task<List<ImageDto>> GetOrDefaultAsync(
        UserImagesQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return _context
            .Images.AsNoTracking()
            .Where(i => i.Status == ImageStatusValue.Available)
            .Where(i => i.UploaderId == query.User.Value)
            .WhereIsAccessible(query.Actor)
            .Select(i => new ImageDto(
                i.Id,
                i.UploaderId,
                i.AlbumId,
                i.Title,
                i.Tags,
                i.UploadedAt,
                i.RemovedAt
            ))
            .ToListAsync(cancellationToken);
    }
}
