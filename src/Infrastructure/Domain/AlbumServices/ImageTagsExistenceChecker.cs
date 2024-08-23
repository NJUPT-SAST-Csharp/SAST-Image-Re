using Domain.AlbumDomain.Exceptions;
using Domain.AlbumDomain.ImageEntity;
using Domain.AlbumDomain.Services;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Domain.AlbumServices
{
    internal sealed class ImageTagsExistenceChecker(DomainDbContext context)
        : IImageTagsExistenceChecker
    {
        private readonly DomainDbContext _context = context;

        public async Task CheckAsync(ImageTags tags, CancellationToken cancellationToken = default)
        {
            bool isExisting = await _context
                .Tags.AsNoTracking()
                .AllAsync(tag => tags.Contains(tag.Id), cancellationToken);

            if (isExisting == false)
            {
                ImageTagsNotFoundException.Throw(tags);
            }
        }
    }
}
