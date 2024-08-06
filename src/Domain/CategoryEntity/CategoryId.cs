using Domain.Internal;

namespace Domain.CategoryEntity
{
    internal readonly record struct CategoryId : ITypedId<CategoryId>
    {
        public readonly long Value { get; }

        public static CategoryId CreateNewId() => new();
    }
}
