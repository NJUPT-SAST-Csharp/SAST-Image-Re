using Domain.AlbumDomain.AlbumEntity;
using Domain.Event;

namespace Domain.AlbumDomain.Events
{
    public readonly record struct AlbumCollaboratorsUpdatedEvent(
        AlbumId Album,
        Collaborators Collaborators
    ) : IDomainEvent { }
}
