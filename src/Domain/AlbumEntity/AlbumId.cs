using Domain.Internal.Entity;

namespace Domain.AlbumEntity
{
    public readonly record struct AlbumId : ITypedId<AlbumId>
    {
        public long Value { get; }

        public static AlbumId CreateNewId() => new();
    }
}
