using System.Diagnostics.CodeAnalysis;
using Domain.Entity;

namespace Domain.AlbumDomain.AlbumEntity
{
    public readonly record struct AccessLevel
        : IValueObject<AccessLevel, AccessLevelValue>,
            IFactoryConstructor<AccessLevel, int>
    {
        public const int MaxValue = (int)AccessLevelValue.Private;
        public const int MinValue = (int)AccessLevelValue.Public;

        public AccessLevelValue Value { get; }

        public static readonly AccessLevel Public = new(AccessLevelValue.Public);
        public static readonly AccessLevel AuthOnly = new(AccessLevelValue.AuthOnly);
        public static readonly AccessLevel Private = new(AccessLevelValue.Private);

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
                (int)AccessLevelValue.Public => Public,
                (int)AccessLevelValue.AuthOnly => AuthOnly,
                (int)AccessLevelValue.Private => Private,
                _ => throw new InvalidOperationException(),
            };
            return true;
        }
    }

    public enum AccessLevelValue
    {
        Public,
        AuthOnly,
        Private,
    }
}
