using Domain.Internal.Entity;

namespace Domain.CategoryEntity
{
    public readonly record struct CategoryId : ITypedId<CategoryId>
    {
        public readonly long Value { get; }

        public static CategoryId CreateNewId() => new();
    }
}
