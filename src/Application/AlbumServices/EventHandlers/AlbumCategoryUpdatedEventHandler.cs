using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Events;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.AlbumServices.EventHandlers
{
    internal sealed class AlbumCategoryUpdatedEventHandler(
        IRepository<AlbumModel, AlbumId> repository
    ) : IDomainEventHandler<AlbumCategoryUpdatedEvent>
    {
        private readonly IRepository<AlbumModel, AlbumId> _repository = repository;

        public async Task Handle(AlbumCategoryUpdatedEvent e, CancellationToken cancellationToken)
        {
            var album = await _repository.GetAsync(e.Album, cancellationToken);

            album.UpdateCategory(e);
        }
    }
}
