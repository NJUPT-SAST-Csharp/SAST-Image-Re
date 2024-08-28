using Domain.AlbumDomain.AlbumEntity;
using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.AlbumDomain.Events
{
    public sealed record class AlbumSubscribedEvent(AlbumId Album, UserId User) : IDomainEvent { }
}
