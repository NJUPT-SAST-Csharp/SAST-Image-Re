using Domain.Internal;

namespace Domain.TagEntity
{
    internal readonly record struct TagName : IValueObject<TagName, string>
    {
        public readonly string Value { get; private init; }

        public static TagName CreateNew(string value)
        {
            return new() { Value = value };
        }
    }
}
