using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Events;
using Domain.Core.Event;
using Domain.Extensions;
using Domain.UserDomain.UserEntity;

namespace Application.AlbumServices.EventHandlers;

internal sealed class AlbumSubscribedEventHandler(
    IRepository<SubscribeModel, (AlbumId, UserId)> repository
) : IDomainEventHandler<AlbumSubscribedEvent>
{
    private readonly IRepository<SubscribeModel, (AlbumId, UserId)> _repository = repository;

    public Task Handle(AlbumSubscribedEvent e, CancellationToken cancellationToken)
    {
        SubscribeModel subscribe = new(e.Album.Value, e.User.Value);

        return _repository.AddAsync(subscribe, cancellationToken);
    }
}
