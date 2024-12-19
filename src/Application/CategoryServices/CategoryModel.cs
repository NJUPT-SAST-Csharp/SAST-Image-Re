using Domain.CategoryDomain.Events;

namespace Application.CategoryServices;

public sealed class CategoryModel
{
    internal CategoryModel(CategoryCreatedEvent e)
    {
        Id = e.Id.Value;
        Name = e.Name.Value;
        Description = e.Description.Value;
    }

    public long Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
}
