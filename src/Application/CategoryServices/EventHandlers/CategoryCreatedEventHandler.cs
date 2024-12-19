using Domain.CategoryDomain.CategoryEntity;
using Domain.CategoryDomain.Events;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.CategoryServices.EventHandlers;

internal sealed class CategoryCreatedEventHandler(IRepository<CategoryModel, CategoryId> repository)
    : IDomainEventHandler<CategoryCreatedEvent>
{
    public Task Handle(CategoryCreatedEvent e, CancellationToken cancellationToken)
    {
        CategoryModel category = new(e);

        return repository.AddAsync(category, cancellationToken);
    }
}
