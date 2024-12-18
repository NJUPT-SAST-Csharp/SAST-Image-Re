using Domain.CategoryDomain.CategoryEntity;
using Domain.Extensions;
using Domain.Shared;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.CategoryServices.Domain;

internal sealed class CategoryDomainRepository(DomainDbContext context)
    : IRepository<Category, CategoryId>
{
    public async Task<CategoryId> AddAsync(
        Category entity,
        CancellationToken cancellationToken = default
    )
    {
        var entry = await context.Categories.AddAsync(entity, cancellationToken);

        return entry.Entity.Id;
    }

    public async Task DeleteAsync(CategoryId id, CancellationToken cancellationToken = default)
    {
        var category = await GetOrDefaultAsync(id, cancellationToken);
        if (category is not null)
            context.Categories.Remove(category);
    }

    public async Task<Category> GetAsync(
        CategoryId id,
        CancellationToken cancellationToken = default
    )
    {
        var category =
            await context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw new EntityNotFoundException();

        return category;
    }

    public Task<Category?> GetOrDefaultAsync(
        CategoryId id,
        CancellationToken cancellationToken = default
    )
    {
        return context.Categories.FirstOrDefaultAsync(
            category => category.Id == id,
            cancellationToken
        );
    }
}
