using Domain.AlbumDomain.ImageEntity;
using Domain.Event;

namespace Domain.AlbumDomain.Events
{
    public sealed record class ImageRemovedEvent(ImageId Image, ImageStatus Status)
        : IDomainEvent { }
}
