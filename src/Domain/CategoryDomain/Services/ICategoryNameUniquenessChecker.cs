using Domain.CategoryDomain.CategoryEntity;

namespace Domain.CategoryDomain.Services;

public interface ICategoryNameUniquenessChecker
{
    public Task CheckAsync(CategoryName name, CancellationToken cancellationToken = default);
}
