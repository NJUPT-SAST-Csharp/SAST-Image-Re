using Domain.Entity;

namespace Domain.TagDomain.TagEntity
{
    public readonly record struct TagId(long Value) : ITypedId<TagId, long>
    {
        public static TagId GenerateNew() => new(Snowflake.NewId);
    }
}
