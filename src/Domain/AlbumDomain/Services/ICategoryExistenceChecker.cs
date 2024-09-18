using Domain.CategoryDomain.CategoryEntity;

namespace Domain.AlbumDomain.Services;

public interface ICategoryExistenceChecker
{
    public Task CheckAsync(CategoryId category, CancellationToken cancellationToken = default);
}
