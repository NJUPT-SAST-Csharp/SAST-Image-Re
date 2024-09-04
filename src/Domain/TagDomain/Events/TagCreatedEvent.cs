using Domain.Event;
using Domain.TagDomain.TagEntity;

namespace Domain.TagDomain.Events
{
    public sealed record TagCreatedEvent(TagId Id, TagName TagName) : IDomainEvent { }
}
