using Domain.Entity;

namespace Domain.TagDomain.TagEntity
{
    public sealed class Tag : EntityBase<TagId>
    {
        private Tag()
            : base(default) { }

        private readonly TagName _name;
    }
}
