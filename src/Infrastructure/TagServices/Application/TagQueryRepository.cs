using Application;
using Application.TagServices.Queries;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TagServices.Application
{
    internal sealed class TagQueryRepository(QueryDbContext context)
        : IQueryRepository<TagsQuery, List<TagDto>>
    {
        private readonly QueryDbContext _context = context;

        public Task<List<TagDto>> GetOrDefaultAsync(
            TagsQuery query,
            CancellationToken cancellationToken = default
        )
        {
            return _context
                .Tags.AsNoTracking()
                .Where(tag => query.Name == null || EF.Functions.ILike(tag.Name, $"%{query.Name}%"))
                .Select(tag => new TagDto(tag.Id, tag.Name))
                .ToListAsync(cancellationToken);
        }
    }
}
