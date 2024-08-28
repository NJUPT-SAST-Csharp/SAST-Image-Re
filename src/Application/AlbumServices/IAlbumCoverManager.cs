﻿using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;

namespace Application.AlbumServices
{
    public interface IAlbumCoverManager
    {
        public Task UpdateWithCustomImageAsync(
            AlbumId album,
            Stream stream,
            CancellationToken cancellationToken = default
        );

        public Task UpdateWithContainedImageAsync(
            AlbumId album,
            ImageId image,
            CancellationToken cancellationToken = default
        );

        public Task RemoveCoverAsync(AlbumId album, CancellationToken cancellationToken = default);

        public Stream? OpenReadStream(AlbumId album);
    }
}