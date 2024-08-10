using System.Diagnostics.CodeAnalysis;
using Domain.Entity;

namespace Domain.TagEntity
{
    public readonly record struct TagName
        : IValueObject<TagName, string>,
            IFactoryConstructor<TagName, string>
    {
        public const int MaxLength = 10;

        public readonly string Value { get; private init; }

        public static bool TryCreateNew(
            string value,
            [MaybeNullWhen(false), NotNullWhen(true)] out TagName newObject
        )
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                newObject = default;
                return false;
            }

            value = value.Trim();

            if (value.Length > MaxLength)
            {
                newObject = default;
                return false;
            }

            newObject = new() { Value = value };
            return true;
        }
    }
}
