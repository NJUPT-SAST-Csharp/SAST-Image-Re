using Domain.ImageEntity;

namespace Domain.AlbumEntity
{
    internal sealed record class AlbumImage(AlbumId Album, ImageId Image);
}
