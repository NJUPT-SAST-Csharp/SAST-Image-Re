using Domain.Entity;

namespace Domain.CategoryEntity
{
    public readonly record struct CategoryId(long Value) : ITypedId<CategoryId, long>
    {
        public static CategoryId GenerateNew() => new(Snowflake.NewId);
    }
}
