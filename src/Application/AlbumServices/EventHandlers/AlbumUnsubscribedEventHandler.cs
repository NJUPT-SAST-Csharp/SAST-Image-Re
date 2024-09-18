using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Events;
using Domain.Core.Event;
using Domain.Extensions;
using Domain.UserDomain.UserEntity;

namespace Application.AlbumServices.EventHandlers;

internal sealed class AlbumUnsubscribedEventHandler(
    IRepository<SubscribeModel, (AlbumId, UserId)> repository
) : IDomainEventHandler<AlbumUnsubscribedEvent>
{
    private readonly IRepository<SubscribeModel, (AlbumId, UserId)> _repository = repository;

    public Task Handle(AlbumUnsubscribedEvent e, CancellationToken cancellationToken)
    {
        return _repository.DeleteAsync((e.Album, e.User), cancellationToken);
    }
}
