using System.Diagnostics.CodeAnalysis;
using Domain.Entity;

namespace Domain.AlbumDomain.AlbumEntity
{
    public readonly record struct Accessibility
        : IValueObject<Accessibility, AccessibilityValue>,
            IFactoryConstructor<Accessibility, int>
    {
        public const int MaxValue = 3;
        public const int MinValue = 1;

        public AccessibilityValue Value { get; }

        public static readonly Accessibility Public = new(AccessibilityValue.Public);
        public static readonly Accessibility AuthOnly = new(AccessibilityValue.AuthOnly);
        public static readonly Accessibility Private = new(AccessibilityValue.Private);

        internal Accessibility(AccessibilityValue value) => Value = value;

        public static bool TryCreateNew(int input, [NotNullWhen(true)] out Accessibility newObject)
        {
            if (input > 3 || input < 1)
            {
                newObject = default;
                return false;
            }

            newObject = input switch
            {
                1 => Public,
                2 => AuthOnly,
                3 => Private,
                _ => throw new InvalidOperationException(),
            };
            return true;
        }
    }

    public enum AccessibilityValue
    {
        Public,
        AuthOnly,
        Private,
    }
}
