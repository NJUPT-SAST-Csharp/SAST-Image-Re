using Domain.Entity;

namespace Domain.CategoryDomain.CategoryEntity
{
    public readonly record struct CategoryId(long Value) : ITypedId<CategoryId, long>
    {
        public static CategoryId GenerateNew() => new(Snowflake.NewId);
    }
}
