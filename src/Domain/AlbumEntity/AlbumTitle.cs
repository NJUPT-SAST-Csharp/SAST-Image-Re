using System.Diagnostics.CodeAnalysis;
using Domain.Entity;

namespace Domain.AlbumEntity
{
    public readonly record struct AlbumTitle
        : IValueObject<AlbumTitle, string>,
            IFactoryConstructor<AlbumTitle, string>
    {
        public const int MaxLength = 20;
        public string Value { get; private init; }

        public static bool TryCreateNew(
            string value,
            [MaybeNullWhen(false), NotNullWhen(true)] out AlbumTitle newObject
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
