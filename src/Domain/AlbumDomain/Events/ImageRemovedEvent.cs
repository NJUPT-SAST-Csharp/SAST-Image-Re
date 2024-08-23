using Domain.AlbumDomain.ImageEntity;
using Domain.Event;

namespace Domain.AlbumDomain.Events
{
    public readonly record struct ImageRemovedEvent(ImageId Image) : IDomainEvent { }
}
