using Domain.CategoryDomain.Commands;
using Domain.CategoryDomain.Events;
using Domain.Entity;
using Domain.Shared;

namespace Domain.CategoryDomain.CategoryEntity;

public sealed class Category : EntityBase<CategoryId>
{
    private Category()
        : base(default) { }

    public Category(CreateCategoryCommand command)
        : base(CategoryId.GenerateNew())
    {
        if (command.Actor.IsAdmin == false)
            throw new NoPermissionException();

        _name = command.Name;
        AddDomainEvent(new CategoryCreatedEvent(Id, command.Name, command.Description));
    }

    private CategoryName _name;

    public void UpdateName(UpdateCategoryNameCommand command)
    {
        if (command.Actor.IsAdmin == false)
            throw new NoPermissionException();

        _name = command.Name;
        AddDomainEvent(new CategoryNameUpdatedEvent(Id, command.Name));
    }
}
