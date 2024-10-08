﻿using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands;

public sealed record class RestoreImageCommand(AlbumId Album, ImageId Image, Actor Actor)
    : IDomainCommand { }

public sealed class RestoreImageCommandHandler(IRepository<Album, AlbumId> repository)
    : IDomainCommandHandler<RestoreImageCommand>
{
    private readonly IRepository<Album, AlbumId> _repository = repository;

    public async Task Handle(RestoreImageCommand request, CancellationToken cancellationToken)
    {
        var album = await _repository.GetAsync(request.Album, cancellationToken);

        album.RestoreImage(request);
    }
}
