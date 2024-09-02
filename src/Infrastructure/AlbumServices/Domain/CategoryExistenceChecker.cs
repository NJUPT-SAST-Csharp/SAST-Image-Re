using Domain.AlbumDomain.Exceptions;
using Domain.AlbumDomain.Services;
using Domain.CategoryDomain.CategoryEntity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AlbumServices.Domain
{
    internal sealed class CategoryExistenceChecker(DomainDbContext context)
        : ICategoryExistenceChecker
    {
        private readonly DomainDbContext _context = context;

        public async Task CheckAsync(
            CategoryId category,
            CancellationToken cancellationToken = default
        )
        {
            bool isExisting = await _context
                .Categories.AsNoTracking()
                .AnyAsync(c => c.Id == category, cancellationToken);

            if (isExisting == false)
            {
                CategoryNotFoundException.Throw(category);
            }
        }
    }
}
