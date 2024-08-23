using Domain.AlbumDomain.AlbumEntity;
using Domain.Event;

namespace Domain.AlbumDomain.Events
{
    public readonly record struct AlbumDescriptionUpdatedEvent(
        AlbumId Album,
        AlbumDescription Description
    ) : IDomainEvent { }
}
