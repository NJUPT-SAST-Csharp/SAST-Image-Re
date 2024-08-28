using Application.ImageServices;
using Domain.AlbumDomain.ImageEntity;
using Domain.Extensions;
using Domain.Shared;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Application.ImageSerivces
{
    internal sealed class ImageModelRepository(QueryDbContext context)
        : IRepository<ImageModel, ImageId>
    {
        private readonly QueryDbContext _context = context;

        public async Task<ImageId> AddAsync(
            ImageModel entity,
            CancellationToken cancellationToken = default
        )
        {
            var imageEntry = await _context.Images.AddAsync(entity, cancellationToken);

            return new(imageEntry.Entity.Id);
        }

        public async Task<ImageModel> GetAsync(
            ImageId id,
            CancellationToken cancellationToken = default
        )
        {
            var image = await _context
                .Images.IgnoreQueryFilters()
                .FirstOrDefaultAsync(image => image.Id == id.Value, cancellationToken);

            if (image is null)
                EntityNotFoundException.Throw(id);

            return image;
        }

        public Task<ImageModel?> GetOrDefaultAsync(
            ImageId id,
            CancellationToken cancellationToken = default
        )
        {
            return _context
                .Images.IgnoreQueryFilters()
                .FirstOrDefaultAsync(image => image.Id == id.Value, cancellationToken);
        }
    }
}
