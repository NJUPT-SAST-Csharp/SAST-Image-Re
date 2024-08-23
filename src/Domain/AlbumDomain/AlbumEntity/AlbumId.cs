using Domain.Entity;

namespace Domain.AlbumDomain.AlbumEntity
{
    public readonly record struct AlbumId(long Value) : ITypedId<AlbumId, long>
    {
        public static AlbumId GenerateNew() => new(Snowflake.NewId);
    }
}
