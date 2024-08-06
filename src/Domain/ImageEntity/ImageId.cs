using Domain.Internal;

namespace Domain.ImageEntity
{
    internal readonly record struct ImageId : ITypedId<ImageId>
    {
        public readonly long Value { get; }

        public static ImageId CreateNewId() => new();
    }
}
