using Domain.CategoryDomain.CategoryEntity;
using Domain.Event;

namespace Domain.CategoryDomain.Events;

public sealed record class CategoryDescriptionUpdatedEvent(
    CategoryId Id,
    CategoryDescription Description
) : IDomainEvent { }
