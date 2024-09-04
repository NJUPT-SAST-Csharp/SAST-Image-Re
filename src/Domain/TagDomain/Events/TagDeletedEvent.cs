using Domain.Event;
using Domain.TagDomain.TagEntity;

namespace Domain.TagDomain.Events
{
    public sealed record TagDeletedEvent(TagId Id) : IDomainEvent { }
}
