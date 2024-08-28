using System.Diagnostics.CodeAnalysis;
using Domain.Entity;

namespace Domain.AlbumDomain.AlbumEntity
{
    public readonly record struct Accessibility
        : IValueObject<Accessibility, AccessibilityValue>,
            IFactoryConstructor<Accessibility, int>
    {
        public const int MaxValue = 2;
        public const int MinValue = 0;

        public AccessibilityValue Value { get; }

        public static readonly Accessibility Public = new(AccessibilityValue.Public);
        public static readonly Accessibility AuthOnly = new(AccessibilityValue.AuthOnly);
        public static readonly Accessibility Private = new(AccessibilityValue.Private);

        internal Accessibility(AccessibilityValue value) => Value = value;

        public static bool TryCreateNew(int input, [NotNullWhen(true)] out Accessibility newObject)
        {
            if (input > 2 || input < 0)
            {
                newObject = default;
                return false;
            }

            newObject = input switch
            {
                0 => Public,
                1 => AuthOnly,
                2 => Private,
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
