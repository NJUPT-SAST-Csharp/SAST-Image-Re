using Domain.CategoryDomain.CategoryEntity;
using Domain.Event;

namespace Domain.CategoryDomain.Events;

public sealed record class CategoryNameUpdatedEvent(CategoryId Id, CategoryName Name)
    : IDomainEvent;
