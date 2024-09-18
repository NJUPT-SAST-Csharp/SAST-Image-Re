using Domain.AlbumDomain.ImageEntity;

namespace Domain.AlbumDomain.AlbumEntity;

public sealed record class Cover(ImageId? Id, bool IsLatestImage)
{
    public static readonly Cover Default = new(null, true);
    public static readonly Cover UserCustomCover = new(null, false);
}
