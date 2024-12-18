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
        CategoryModel category =
            new()
            {
                Id = e.Id.Value,
                Description = e.Description.Value,
                Name = e.Name.Value,
            };

        return repository.AddAsync(category, cancellationToken);
    }
}
