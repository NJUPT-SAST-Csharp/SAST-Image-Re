using Application.AlbumServices;
using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Infrastructure.Storage;

namespace Infrastructure.Application.AlbumServices
{
    sealed class AlbumCoverManager(IStorageManager manager) : IAlbumCoverManager
    {
        private readonly IStorageManager _manager = manager;

        public Stream? OpenReadStream(AlbumId album)
        {
            string id = album.Value.ToString();
            return _manager.FindFile(id, StorageKind.Cover);
        }

        public Task RemoveCoverAsync(AlbumId album, CancellationToken cancellationToken = default)
        {
            var id = album.Value.ToString();
            return _manager.DeleteAsync(id, cancellationToken);
        }

        public async Task UpdateWithContainedImageAsync(
            AlbumId album,
            ImageId image,
            CancellationToken cancellationToken = default
        )
        {
            string imageId = image.Value.ToString();
            await using var stream =
                _manager.FindFile(imageId, StorageKind.Image)
                ?? throw new FileNotFoundException(null, imageId);

            string albumId = album.Value.ToString();
            await _manager.StoreAsync(albumId, stream, StorageKind.Cover, cancellationToken);
        }

        public async Task UpdateWithCustomImageAsync(
            AlbumId album,
            Stream stream,
            CancellationToken cancellationToken = default
        )
        {
            string id = album.Value.ToString();
            await _manager.StoreAsync(id, stream, StorageKind.Cover, cancellationToken);
            await stream.DisposeAsync();
        }
    }
}
