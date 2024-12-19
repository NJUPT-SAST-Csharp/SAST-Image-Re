using Domain.CategoryDomain.CategoryEntity;
using Domain.CategoryDomain.Events;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.CategoryServices.EventHandlers;

internal sealed class CategoryNameUpdatedEventHandler(
    IRepository<CategoryModel, CategoryId> repository
) : IDomainEventHandler<CategoryNameUpdatedEvent>
{
    public async Task Handle(CategoryNameUpdatedEvent e, CancellationToken cancellationToken)
    {
        var category = await repository.GetAsync(e.Id, cancellationToken);

        category.UpdateName(e);
    }
}
