using Domain.TagDomain;
using Domain.TagDomain.Services;
using Domain.TagDomain.TagEntity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TagServices.Domain
{
    internal sealed class TagNameUniquenessChecker(DomainDbContext context)
        : ITagNameUniquenessChecker
    {
        private readonly DomainDbContext _context = context;

        public async Task CheckAsync(TagName name, CancellationToken cancellationToken = default)
        {
            bool isDuplicated = await _context
                .Tags.Select(tag => EF.Property<TagName>(tag, "_name"))
                .ContainsAsync(name, cancellationToken);

            if (isDuplicated)
                throw new TagNameDuplicateException(name);
        }
    }
}
