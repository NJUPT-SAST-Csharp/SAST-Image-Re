using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Events;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.AlbumServices.EventHandlers
{
    internal sealed class AlbumCollaboratorsUpdatedEventHandler(
        IRepository<AlbumModel, AlbumId> repository
    ) : IDomainEventHandler<AlbumCollaboratorsUpdatedEvent>
    {
        private readonly IRepository<AlbumModel, AlbumId> _repository = repository;

        public async Task Handle(
            AlbumCollaboratorsUpdatedEvent notification,
            CancellationToken cancellationToken
        )
        {
            var album = await _repository.GetAsync(notification.Album, cancellationToken);

            album.Collaborators = notification.Collaborators.Value.Select(c => c.Value).ToArray();
        }
    }
}
