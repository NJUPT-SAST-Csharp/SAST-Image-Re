using Domain.AlbumDomain.ImageEntity;
using Domain.Event;

namespace Domain.AlbumDomain.Events
{
    public sealed record ImageDeletedEvent(ImageId Image) : IDomainEvent { }
}
