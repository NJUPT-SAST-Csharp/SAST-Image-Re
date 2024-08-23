﻿using Domain.AlbumDomain.AlbumEntity;
using Domain.Event;
using Domain.UserDomain.UserEntity;

namespace Domain.AlbumDomain.Events
{
    public readonly record struct AlbumSubscribedEvent(AlbumId Album, UserId User)
        : IDomainEvent { }
}
