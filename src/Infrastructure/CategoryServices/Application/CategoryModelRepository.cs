using Application.CategoryServices;
using Domain.CategoryDomain.CategoryEntity;
using Domain.Extensions;
using Domain.Shared;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.CategoryServices.Application;

internal sealed class CategoryModelRepository(QueryDbContext context)
    : IRepository<CategoryModel, CategoryId>
{
    public async Task<CategoryId> AddAsync(
        CategoryModel entity,
        CancellationToken cancellationToken = default
    )
    {
        var entry = await context.Categories.AddAsync(entity, cancellationToken);
        return new(entry.Entity.Id);
    }

    public async Task DeleteAsync(CategoryId id, CancellationToken cancellationToken = default)
    {
        var category = await GetOrDefaultAsync(id, cancellationToken);
        if (category is not null)
            context.Categories.Remove(category);
    }

    public async Task<CategoryModel> GetAsync(
        CategoryId id,
        CancellationToken cancellationToken = default
    )
    {
        return await GetOrDefaultAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException();
    }

    public Task<CategoryModel?> GetOrDefaultAsync(
        CategoryId id,
        CancellationToken cancellationToken = default
    )
    {
        return context.Categories.FirstOrDefaultAsync(
            category => category.Id == id.Value,
            cancellationToken
        );
    }
}
