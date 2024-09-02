using Application.ImageServices;
using Domain.AlbumDomain.ImageEntity;
using Infrastructure.SharedServices.Storage;
using SkiaSharp;

namespace Infrastructure.ImageServices.Application
{
    internal sealed class ImageStorageManager(
        IStorageManager manager,
        ICompressProcessor compressor
    ) : IImageStorageManager
    {
        private readonly IStorageManager _manager = manager;
        private readonly ICompressProcessor _compressor = compressor;

        public Task DeleteImageAsync(ImageId image, CancellationToken cancellationToken = default)
        {
            string filename = image.Value.ToString();
            return _manager.DeleteAsync(filename, StorageKind.Image, cancellationToken);
        }

        public Stream? OpenReadStream(ImageId image, ImageKind kind)
        {
            string id = kind switch
            {
                ImageKind.Original => image.Value.ToString(),
                ImageKind.Thumbnail => image.Value.ToString() + "_compressed",
                _ => throw new ArgumentException("Invalid ImageKind", kind.ToString()),
            };

            return _manager.FindFile(id, StorageKind.Image);
        }

        public async Task StoreImageAsync(
            ImageId imageId,
            Stream imageFile,
            CancellationToken cancellationToken = default
        )
        {
            string id = imageId.Value.ToString();
            using SKCodec code = SKCodec.Create(imageFile);
            string format = code.EncodedFormat.ToString().ToLowerInvariant();
            string filename = Path.ChangeExtension(id, format);
            imageFile.Seek(0, SeekOrigin.Begin);
            await _manager.StoreAsync(filename, imageFile, StorageKind.Image, cancellationToken);

            string targetFilename = Path.ChangeExtension(filename, ".webp")
                .Insert(id.Length, "_compressed");

            await using var compressed = await _compressor.CompressAsync(
                imageFile,
                60,
                cancellationToken
            );

            await _manager.StoreAsync(
                targetFilename,
                compressed,
                StorageKind.Image,
                cancellationToken
            );
        }
    }
}
