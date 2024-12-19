using Domain.CategoryDomain.CategoryEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.CategoryDomain.Commands;

public sealed record class UpdateCategoryNameCommand(CategoryId Id, CategoryName Name, Actor Actor)
    : IDomainCommand;

internal sealed class UpdateCategoryNameCommandHandler(IRepository<Category, CategoryId> repository)
    : IDomainCommandHandler<UpdateCategoryNameCommand>
{
    public async Task Handle(UpdateCategoryNameCommand command, CancellationToken cancellationToken)
    {
        var category = await repository.GetAsync(command.Id, cancellationToken);

        category.UpdateName(command);
    }
}
