﻿using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands;

public sealed record class RemoveImageCommand(AlbumId Album, ImageId Image, Actor Actor)
    : IDomainCommand { }

internal sealed class DeleteImageCommandHandler(IRepository<Album, AlbumId> repository)
    : IDomainCommandHandler<RemoveImageCommand>
{
    private readonly IRepository<Album, AlbumId> _repository = repository;

    public async Task Handle(RemoveImageCommand request, CancellationToken cancellationToken)
    {
        var album = await _repository.GetAsync(request.Album, cancellationToken);

        album.RemoveImage(request);
    }
}
