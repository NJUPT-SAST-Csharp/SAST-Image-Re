using Domain.AlbumDomain.ImageEntity;
using Domain.Event;

namespace Domain.AlbumDomain.Events
{
    public readonly record struct ImageRestoredEvent(ImageId Image) : IDomainEvent { }
}
