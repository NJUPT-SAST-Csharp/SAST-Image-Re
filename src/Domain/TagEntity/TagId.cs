using Domain.Internal.Entity;

namespace Domain.TagEntity
{
    internal readonly record struct TagId : ITypedId<TagId>
    {
        public readonly long Value { get; }

        public static TagId CreateNewId() => new();
    }
}
