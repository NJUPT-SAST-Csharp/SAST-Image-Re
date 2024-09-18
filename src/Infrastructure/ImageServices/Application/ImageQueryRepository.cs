using Application;
using Application.ImageServices.Queries;
using Domain.AlbumDomain.ImageEntity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImageServices.Application;

internal sealed class ImageQueryRepository(QueryDbContext context)
    : IQueryRepository<AlbumImagesQuery, List<AlbumImageDto>>,
        IQueryRepository<RemovedImagesQuery, List<RemovedImageDto>>,
        IQueryRepository<DetailedImageQuery, DetailedImage?>
{
    private readonly QueryDbContext _context = context;

    public Task<List<AlbumImageDto>> GetOrDefaultAsync(
        AlbumImagesQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return _context
            .Images.AsNoTracking()
            .Where(i => i.Status == ImageStatusValue.Available)
            .Where(i => i.AlbumId == query.Album.Value)
            .WhereIsAccessible(query.Actor)
            .Select(i => new AlbumImageDto(i.Id, i.Title, i.Tags))
            .ToListAsync(cancellationToken);
    }

    public Task<List<RemovedImageDto>> GetOrDefaultAsync(
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
            .Select(i => new RemovedImageDto(i.Id, i.Title, i.RemovedAt!.Value))
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
}
