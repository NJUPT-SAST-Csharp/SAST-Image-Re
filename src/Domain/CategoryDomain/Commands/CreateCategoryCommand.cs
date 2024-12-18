using Domain.CategoryDomain.CategoryEntity;
using Domain.CategoryDomain.Services;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.CategoryDomain.Commands;

public sealed record class CreateCategoryCommand(
    CategoryName Name,
    CategoryDescription Description,
    Actor Actor
) : IDomainCommand<CategoryId>;

internal sealed class CreateCategoryCommandHandler(
    IRepository<Category, CategoryId> repository,
    ICategoryNameUniquenessChecker checker
) : IDomainCommandHandler<CreateCategoryCommand, CategoryId>
{
    public async Task<CategoryId> Handle(
        CreateCategoryCommand command,
        CancellationToken cancellationToken
    )
    {
        await checker.CheckAsync(command.Name, cancellationToken);

        var category = new Category(command);
        await repository.AddAsync(category, cancellationToken);

        return category.Id;
    }
}
