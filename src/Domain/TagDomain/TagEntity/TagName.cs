using System.Diagnostics.CodeAnalysis;
using Domain.Entity;

namespace Domain.TagDomain.TagEntity
{
    public readonly record struct TagName
        : IValueObject<TagName, string>,
            IFactoryConstructor<TagName, string>
    {
        public const int MaxLength = 10;
        public const int MinLength = 2;

        public readonly string Value { get; }

        internal TagName(string value) => Value = value;

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

            if (value.Length > MaxLength || value.Length < MinLength)
            {
                newObject = default;
                return false;
            }

            newObject = new(value);
            return true;
        }
    }
}
