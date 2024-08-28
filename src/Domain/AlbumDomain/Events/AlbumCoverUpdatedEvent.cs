using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Domain.Event;

namespace Domain.AlbumDomain.Events
{
    public sealed record class AlbumCoverUpdatedEvent(
        AlbumId Album,
        ImageId? ContainedImage,
        Stream? CoverImage
    ) : IDomainEvent
    {
        public static AlbumCoverUpdatedEvent ContainedImageOrEmpty(
            AlbumId album,
            ImageId? containedImage
        ) => new(album, containedImage, null);

        public static AlbumCoverUpdatedEvent UserCustomImage(AlbumId album, Stream coverImage) =>
            new(album, null, coverImage);

        public static AlbumCoverUpdatedEvent NewAddedImage(AlbumId album, Stream imageFile) =>
            new(album, null, imageFile);
    }
}
