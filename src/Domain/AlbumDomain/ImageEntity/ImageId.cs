using Domain.Entity;

namespace Domain.AlbumDomain.ImageEntity
{
    public readonly record struct ImageId(long Value) : ITypedId<ImageId, long>
    {
        public static ImageId GenerateNew() => new(Snowflake.NewId);
    }
}
