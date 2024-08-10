using System.Diagnostics.CodeAnalysis;
using Domain.Entity;

namespace Domain.ImageEntity
{
    public readonly record struct ImageTitle
        : IValueObject<ImageTitle, string>,
            IFactoryConstructor<ImageTitle, string>
    {
        public const int MaxLength = 20;

        public string Value { get; private init; }

        public static bool TryCreateNew(
            string value,
            [MaybeNullWhen(false), NotNullWhen(true)] out ImageTitle newObject
        )
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                newObject = new() { Value = string.Empty };
                return true;
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
