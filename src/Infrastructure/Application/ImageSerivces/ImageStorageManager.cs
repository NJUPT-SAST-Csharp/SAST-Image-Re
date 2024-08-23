using Application.ImageServices;
using Domain.AlbumDomain.ImageEntity;
using Infrastructure.Storage;
using Microsoft.Extensions.Options;

namespace Infrastructure.Application.ImageSerivces
{
    internal sealed class ImageStorageManager(
        IOptions<StorageOptions> options,
        IStorageManager manager
    ) : IImageStorageManager
    {
        private readonly IStorageManager _manager = manager;
        private readonly StorageOptions _options = options.Value;

        public Task AddImageAsync(
            ImageId imageId,
            Stream imageFile,
            CancellationToken cancellationToken = default
        )
        {
            var path = Path.Combine(_options.ImagePath, imageId.Value.ToString());

            return _manager.StoreAsync(path, imageFile, cancellationToken);
        }
    }
}
