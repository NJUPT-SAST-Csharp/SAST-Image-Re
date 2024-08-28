using Application.ImageServices;
using Domain.AlbumDomain.ImageEntity;
using Infrastructure.Storage;

namespace Infrastructure.Application.ImageSerivces
{
    internal sealed class ImageStorageManager(IStorageManager manager) : IImageStorageManager
    {
        private readonly IStorageManager _manager = manager;

        public Task AddImageAsync(
            ImageId imageId,
            Stream imageFile,
            CancellationToken cancellationToken = default
        )
        {
            var id = imageId.Value.ToString();

            return _manager.StoreAsync(id, imageFile, StorageKind.Image, cancellationToken);
        }
    }
}
