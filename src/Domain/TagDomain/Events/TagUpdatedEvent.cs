using Domain.Event;
using Domain.TagDomain.TagEntity;

namespace Domain.TagDomain.Events
{
    public sealed record TagUpdatedEvent(TagId Id, TagName NewName) : IDomainEvent { }
}
