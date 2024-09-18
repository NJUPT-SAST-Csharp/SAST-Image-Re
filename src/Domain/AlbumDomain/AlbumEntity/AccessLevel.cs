using System.Diagnostics.CodeAnalysis;
using Domain.Entity;

namespace Domain.AlbumDomain.AlbumEntity;

public readonly record struct AccessLevel
    : IValueObject<AccessLevel, AccessLevelValue>,
        IFactoryConstructor<AccessLevel, int>,
        IComparable<AccessLevel>
{
    public const int MinValue = (int)AccessLevelValue.Private;
    public const int MaxValue = (int)AccessLevelValue.PublicReadWrite;

    public AccessLevelValue Value { get; }

    public static readonly AccessLevel Private = new(AccessLevelValue.Private);
    public static readonly AccessLevel AuthReadOnly = new(AccessLevelValue.AuthReadOnly);
    public static readonly AccessLevel AuthReadWrite = new(AccessLevelValue.AuthReadWrite);
    public static readonly AccessLevel PublicReadOnly = new(AccessLevelValue.PublicReadOnly);
    public static readonly AccessLevel PublicReadWrite = new(AccessLevelValue.PublicReadWrite);

    internal AccessLevel(AccessLevelValue value) => Value = value;

    public static bool TryCreateNew(int input, [NotNullWhen(true)] out AccessLevel newObject)
    {
        if (input > MaxValue || input < MinValue)
        {
            newObject = default;
            return false;
        }

        newObject = input switch
        {
            (int)AccessLevelValue.Private => Private,
            (int)AccessLevelValue.AuthReadOnly => AuthReadOnly,
            (int)AccessLevelValue.AuthReadWrite => AuthReadWrite,
            (int)AccessLevelValue.PublicReadOnly => PublicReadOnly,
            (int)AccessLevelValue.PublicReadWrite => PublicReadWrite,
            _ => throw new InvalidOperationException(),
        };
        return true;
    }

    public readonly int CompareTo(AccessLevel other)
    {
        return Value.CompareTo(other.Value);
    }

    public readonly bool OthersCanWrite =>
        Value == AccessLevelValue.PublicReadWrite || Value == AccessLevelValue.AuthReadWrite;
    public readonly bool OthersCanNotWrite => !OthersCanWrite;

    public static bool operator <(AccessLevel left, AccessLevel right) => left.CompareTo(right) < 0;

    public static bool operator >(AccessLevel left, AccessLevel right) => left.CompareTo(right) > 0;
}

public enum AccessLevelValue
{
    Private,
    AuthReadOnly,
    AuthReadWrite,
    PublicReadOnly,
    PublicReadWrite,
}
