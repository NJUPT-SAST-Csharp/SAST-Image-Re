using Application.AlbumServices;
using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Infrastructure.Storage;

namespace Infrastructure.Application.AlbumServices
{
    internal sealed class CoverStorageManager(
        IStorageManager manager,
        ICompressProcessor compressor
    ) : ICoverStorageManager
    {
        private readonly IStorageManager _manager = manager;
        private readonly ICompressProcessor _compressor = compressor;

        public Stream? OpenReadStream(AlbumId album)
        {
            string id = album.Value.ToString();
            return _manager.FindFile(id, StorageKind.Cover);
        }

        public Task DeleteCoverAsync(AlbumId album, CancellationToken cancellationToken = default)
        {
            var id = album.Value.ToString();
            return _manager.DeleteAsync(id, StorageKind.Cover, cancellationToken);
        }

        public async Task UpdateWithContainedImageAsync(
            AlbumId album,
            ImageId image,
            CancellationToken cancellationToken = default
        )
        {
            string imageId = image.Value.ToString() + "_compressed";
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
            await using var _ = stream;
            await using var compressed = await _compressor.CompressAsync(
                stream,
                90,
                cancellationToken
            );

            await _manager.StoreAsync(id, compressed, StorageKind.Cover, cancellationToken);
        }
    }
}
