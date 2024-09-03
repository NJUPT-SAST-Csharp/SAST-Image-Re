using System.Diagnostics.CodeAnalysis;
using Domain.Entity;

namespace Domain.AlbumDomain.ImageEntity
{
    public readonly struct ImageTags
        : IValueObject<ImageTags, string[]>,
            IFactoryConstructor<ImageTags, string[]?>
    {
        public const int MaxCount = 10;

        public const int MaxLength = 10;
        public const int MinLength = 1;

        public string[] Value { get; }

        internal ImageTags(string[] value)
        {
            Value = value;
        }

        public static bool TryCreateNew(
            string[]? input,
            [NotNullWhen(true)] out ImageTags newObject
        )
        {
            if (input is null || input.Length == 0)
            {
                newObject = new ImageTags([]);
                return true;
            }

            var value = Array.ConvertAll(input, i => i.Trim()).Distinct().ToArray();

            if (value.Length > MaxCount)
            {
                newObject = default;
                return false;
            }

            if (value.Any(tag => tag.Length > MaxLength || tag.Length < MinLength))
            {
                newObject = default;
                return false;
            }

            newObject = new(value);
            return true;
        }

        public bool Equals(ImageTags other)
        {
            return Value.SequenceEqual(other.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is ImageTags tags && Equals(tags);
        }

        public static bool operator ==(ImageTags left, ImageTags right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ImageTags left, ImageTags right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
