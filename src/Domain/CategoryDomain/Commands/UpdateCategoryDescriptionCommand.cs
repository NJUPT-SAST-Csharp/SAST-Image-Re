using Domain.CategoryDomain.CategoryEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.CategoryDomain.Commands;

public sealed record class UpdateCategoryDescriptionCommand(
    CategoryId Id,
    CategoryDescription Description,
    Actor Actor
) : IDomainCommand;

internal sealed class UpdateCategoryDescriptionCommandHandler(
    IRepository<Category, CategoryId> repository
) : IDomainCommandHandler<UpdateCategoryDescriptionCommand>
{
    public async Task Handle(
        UpdateCategoryDescriptionCommand command,
        CancellationToken cancellationToken
    )
    {
        var category = await repository.GetAsync(command.Id, cancellationToken);

        category.UpdateDescription(command);
    }
}
