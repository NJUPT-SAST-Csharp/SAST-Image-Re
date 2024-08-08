using Domain.Internal.Entity;

namespace Domain.ImageEntity
{
    public readonly record struct ImageId : ITypedId<ImageId>
    {
        public readonly long Value { get; }

        public static ImageId CreateNewId() => new();
    }
}
