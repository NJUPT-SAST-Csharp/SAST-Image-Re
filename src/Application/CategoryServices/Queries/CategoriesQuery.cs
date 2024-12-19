using Application.Query;

namespace Application.CategoryServices.Queries;

public record class CategoryDto(long Id, string Name, string Description);

public record class CategoriesQuery() : IQueryRequest<CategoryDto[]>;

internal sealed class CategoriesQueryHandler(
    IQueryRepository<CategoriesQuery, CategoryDto[]> repository
) : IQueryRequestHandler<CategoriesQuery, CategoryDto[]>
{
    private readonly IQueryRepository<CategoriesQuery, CategoryDto[]> _repository = repository;

    public Task<CategoryDto[]> Handle(CategoriesQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
