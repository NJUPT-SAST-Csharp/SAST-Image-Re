using Domain.Entity;
using Domain.TagDomain.Commands;
using Domain.TagDomain.Events;

namespace Domain.TagDomain.TagEntity
{
    public sealed class Tag : EntityBase<TagId>
    {
        private Tag()
            : base(default) { }

        private readonly TagName _name;

        public Tag(CreateTagCommand command)
            : base(TagId.GenerateNew())
        {
            _name = command.Name;

            AddDomainEvent(new TagCreatedEvent(Id, _name));
        }
    }
}
