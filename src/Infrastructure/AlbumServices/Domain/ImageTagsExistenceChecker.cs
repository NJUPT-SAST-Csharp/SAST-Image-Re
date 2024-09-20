using Domain.AlbumDomain.Exceptions;
using Domain.AlbumDomain.ImageEntity;
using Domain.AlbumDomain.Services;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AlbumServices.Domain;

internal sealed class ImageTagsExistenceChecker(DomainDbContext context)
    : IImageTagsExistenceChecker
{
    private readonly DomainDbContext _context = context;

    public async Task CheckAsync(ImageTags tags, CancellationToken cancellationToken = default)
    {
        var tagIdsInDb = await _context
            .Tags.AsNoTracking()
            .Select(t => t.Id)
            .Where(t => tags.Contains(t))
            .ToListAsync(cancellationToken);

        bool isExisting = tags.All(tagIdsInDb.Contains);

        if (isExisting == false)
        {
            ImageTagsNotFoundException.Throw(tags);
        }
    }
}
