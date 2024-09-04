using Application.TagServices;
using Domain.Extensions;
using Domain.Shared;
using Domain.TagDomain.TagEntity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TagServices.Application
{
    internal sealed class TagModelRepository(QueryDbContext context) : IRepository<TagModel, TagId>
    {
        private readonly QueryDbContext _context = context;

        public async Task<TagId> AddAsync(
            TagModel entity,
            CancellationToken cancellationToken = default
        )
        {
            var entry = await _context.Tags.AddAsync(entity, cancellationToken);

            return new(entry.Entity.Id);
        }

        public async Task DeleteAsync(TagId id, CancellationToken cancellationToken = default)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(
                tag => tag.Id == id.Value,
                cancellationToken
            );

            if (tag is not null)
            {
                _context.Tags.Remove(tag);
            }
        }

        public async Task<TagModel> GetAsync(
            TagId id,
            CancellationToken cancellationToken = default
        )
        {
            var image =
                await _context.Tags.FirstOrDefaultAsync(
                    tag => tag.Id == id.Value,
                    cancellationToken
                ) ?? throw new EntityNotFoundException();

            return image;
        }
    }
}
