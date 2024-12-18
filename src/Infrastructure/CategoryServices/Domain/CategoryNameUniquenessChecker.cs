﻿using Domain.CategoryDomain.CategoryEntity;
using Domain.CategoryDomain.Exceptions;
using Domain.CategoryDomain.Services;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.CategoryServices.Domain;

internal sealed class CategoryNameUniquenessChecker(DomainDbContext context)
    : ICategoryNameUniquenessChecker
{
    public async Task CheckAsync(CategoryName name, CancellationToken cancellationToken = default)
    {
        bool isDuplicated = await context
            .Albums.FromSql($"SELECT 1 FROM domain.categories WHERE name ILIKE {name.Value}")
            .AsNoTracking()
            .AnyAsync(cancellationToken);
        if (isDuplicated)
            throw new CategoryNameDuplicateException(name);
    }
}
