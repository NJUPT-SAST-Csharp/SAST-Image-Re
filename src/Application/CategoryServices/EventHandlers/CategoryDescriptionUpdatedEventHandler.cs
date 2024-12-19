using Domain.CategoryDomain.CategoryEntity;
using Domain.CategoryDomain.Events;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.CategoryServices.EventHandlers;

internal sealed class CategoryDescriptionUpdatedEventHandler(
    IRepository<CategoryModel, CategoryId> repository
) : IDomainEventHandler<CategoryDescriptionUpdatedEvent>
{
    public async Task Handle(CategoryDescriptionUpdatedEvent e, CancellationToken cancellationToken)
    {
        var category = await repository.GetAsync(e.Id, cancellationToken);

        category.UpdateDescription(e);
    }
}
