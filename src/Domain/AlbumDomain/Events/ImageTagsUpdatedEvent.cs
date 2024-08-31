using Domain.AlbumDomain.ImageEntity;
using Domain.Event;

namespace Domain.AlbumDomain.Events
{
    public sealed record ImageTagsUpdatedEvent(ImageId Image, ImageTags Tags) : IDomainEvent { }
}
