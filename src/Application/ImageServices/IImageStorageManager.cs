using Domain.AlbumDomain.ImageEntity;

namespace Application.ImageServices
{
    public interface IImageStorageManager
    {
        public Task AddImageAsync(
            ImageId imageId,
            Stream imageFile,
            CancellationToken cancellationToken = default
        );
    }
}
