﻿using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Events;
using Domain.Core.Event;
using Domain.Extensions;

namespace Application.AlbumServices.EventHandlers
{
    internal sealed class AlbumTitleUpdatedEventHandler(IRepository<AlbumModel, AlbumId> repository)
        : IDomainEventHandler<AlbumTitleUpdatedEvent>
    {
        private readonly IRepository<AlbumModel, AlbumId> _repository = repository;

        public async Task Handle(
            AlbumTitleUpdatedEvent notification,
            CancellationToken cancellationToken
        )
        {
            var album = await _repository.GetAsync(notification.Album, cancellationToken);

            album.Title = notification.Title.Value;
        }
    }
}