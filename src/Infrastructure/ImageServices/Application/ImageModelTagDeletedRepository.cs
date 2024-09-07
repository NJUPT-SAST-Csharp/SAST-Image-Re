using Application.ImageServices;
using Domain.TagDomain.TagEntity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImageServices.Application
{
    internal sealed class ImageModelTagDeletedRepository(QueryDbContext context)
        : IImageModelTagDeletedRepository
    {
        private readonly QueryDbContext _context = context;

        public Task<List<ImageModel>> GetAsync(TagId id, CancellationToken cancellationToken)
        {
            return _context
                .Images.Where(image => image.Tags.Contains(id.Value))
                .ToListAsync(cancellationToken);
        }
    }
}
