using System.Diagnostics.CodeAnalysis;
using Domain.Entity;

namespace Domain.AlbumDomain.AlbumEntity;

public readonly record struct AlbumDescription
    : IValueObject<AlbumDescription, string>,
        IFactoryConstructor<AlbumDescription, string>
{
    public const int MinLength = 3;
    public const int MaxLength = 60;

    public readonly string Value { get; }

    internal AlbumDescription(string value)
    {
        Value = value;
    }

    public static bool TryCreateNew(
        string value,
        [MaybeNullWhen(false), NotNullWhen(true)] out AlbumDescription newObject
    )
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            newObject = default;
            return false;
        }

        value = value.Trim();

        if (value.Length < MinLength || value.Length > MaxLength)
        {
            newObject = default;
            return false;
        }

        newObject = new AlbumDescription(value);
        return true;
    }
}
