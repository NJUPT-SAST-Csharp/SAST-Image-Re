using Application.AlbumServices;
using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Infrastructure.Storage;
using Microsoft.Extensions.Options;

namespace Infrastructure.Application.AlbumServices
{
    sealed class AlbumCoverManager(IOptions<StorageOptions> options, IStorageManager manager)
        : IAlbumCoverManager
    {
        private readonly IStorageManager _manager = manager;
        private readonly StorageOptions _options = options.Value;

        public Task RemoveCover(AlbumId album, CancellationToken cancellationToken = default)
        {
            string path = Path.Combine(_options.CoverPath, album.Value.ToString());

            return _manager.DeleteAsync(path, cancellationToken);
        }

        public async Task UpdateWithContainedImageAsync(
            AlbumId album,
            ImageId image,
            CancellationToken cancellationToken = default
        )
        {
            string imagePath = Path.Combine(_options.ImagePath, image.Value.ToString());
            string coverPath = Path.Combine(_options.CoverPath, album.Value.ToString());

            await using var imageStream = File.OpenRead(imagePath);

            await _manager.StoreAsync(coverPath, imageStream, cancellationToken);
        }

        public Task UpdateWithCustomImageAsync(
            AlbumId album,
            Stream stream,
            CancellationToken cancellationToken = default
        )
        {
            string path = Path.Combine(_options.CoverPath, album.Value.ToString());

            return _manager.StoreAsync(path, stream, cancellationToken);
        }
    }
}
