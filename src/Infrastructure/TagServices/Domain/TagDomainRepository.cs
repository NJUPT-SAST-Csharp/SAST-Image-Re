using Domain.Extensions;
using Domain.Shared;
using Domain.TagDomain.Events;
using Domain.TagDomain.TagEntity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TagServices.Domain;

internal sealed class TagDomainRepository(DomainDbContext context) : IRepository<Tag, TagId>
{
    private readonly DomainDbContext _context = context;

    public async Task<TagId> AddAsync(Tag entity, CancellationToken cancellationToken = default)
    {
        var entry = await _context.Tags.AddAsync(entity, cancellationToken);

        return entry.Entity.Id;
    }

    public async Task DeleteAsync(TagId id, CancellationToken cancellationToken = default)
    {
        Tag? tag = await _context.Tags.FirstOrDefaultAsync(tag => tag.Id == id, cancellationToken);

        if (tag is not null)
        {
            tag.AddDomainEvent(new TagDeletedEvent(tag.Id));
            _context.Remove(tag);
        }
    }

    public async Task<Tag> GetAsync(TagId id, CancellationToken cancellationToken = default)
    {
        var tag = await GetOrDefaultAsync(id, cancellationToken);

        if (tag is not null)
            return tag;

        throw new EntityNotFoundException();
    }

    public Task<Tag?> GetOrDefaultAsync(TagId id, CancellationToken cancellationToken = default)
    {
        return _context.Tags.FirstOrDefaultAsync(tag => tag.Id == id, cancellationToken);
    }
}
