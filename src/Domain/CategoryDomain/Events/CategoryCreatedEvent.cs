using Domain.CategoryDomain.CategoryEntity;
using Domain.Event;

namespace Domain.CategoryDomain.Events;

public sealed record class CategoryCreatedEvent(
    CategoryId Id,
    CategoryName Name,
    CategoryDescription Description
) : IDomainEvent { }
