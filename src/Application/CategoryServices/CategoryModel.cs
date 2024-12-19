using Domain.CategoryDomain.Events;

namespace Application.CategoryServices;

public sealed class CategoryModel
{
    private CategoryModel() { }

    internal CategoryModel(CategoryCreatedEvent e)
    {
        Id = e.Id.Value;
        Name = e.Name.Value;
        Description = e.Description.Value;
    }

    public long Id { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
}
