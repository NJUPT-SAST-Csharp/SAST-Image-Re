using Domain.Entity;

namespace Domain.TagEntity
{
    internal readonly record struct TagId(long Value) : ITypedId<TagId, long>
    {
        public static TagId GenerateNew() => new(Snowflake.NewId);
    }
}
