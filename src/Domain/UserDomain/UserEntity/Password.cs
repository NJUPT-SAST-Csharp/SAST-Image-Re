using System.Diagnostics.CodeAnalysis;
using Domain.Entity;
using Domain.Shared;

namespace Domain.UserDomain.UserEntity
{
    public readonly record struct Password
    {
        public Password() { }

        internal Password(byte[] hash, byte[] salt)
        {
            Hash = hash;
            Salt = salt;
        }

        public readonly byte[] Hash { get; } = null!;
        public readonly byte[] Salt { get; } = null!;
    }

    public readonly record struct PasswordInput
        : IValueObject<PasswordInput, string>,
            IFactoryConstructor<PasswordInput, string>
    {
        public const int MaxLength = 20;
        public const int MinLength = 6;

        public string Value { get; }

        internal PasswordInput(string value)
        {
            Value = value;
        }

        public static bool TryCreateNew(
            string input,
            [NotNullWhen(true)] out PasswordInput newObject
        )
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                newObject = default;
                return false;
            }

            input = input.Trim();

            if (input.Length < MinLength || input.Length > MaxLength)
            {
                newObject = default;
                return false;
            }

            if (input.IsLetterOrDigitOrUnderline() == false)
            {
                newObject = default;
                return false;
            }

            newObject = new(input);
            return true;
        }
    }
}
