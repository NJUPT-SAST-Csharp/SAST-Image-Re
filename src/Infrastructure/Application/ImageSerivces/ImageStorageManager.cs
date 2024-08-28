using Application.ImageServices;
using Domain.AlbumDomain.ImageEntity;
using Infrastructure.Storage;
using SkiaSharp;

namespace Infrastructure.Application.ImageSerivces
{
    internal sealed class ImageStorageManager(IStorageManager manager) : IImageStorageManager
    {
        private readonly IStorageManager _manager = manager;

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

            string target = Path.ChangeExtension(filename, ".webp")
                .Insert(id.Length, "_compressed");
            imageFile.Seek(0, SeekOrigin.Begin);
            using var image = SKBitmap.Decode(imageFile);
            using var data = image.PeekPixels();
            using var encoded = data.Encode(SKEncodedImageFormat.Webp, 50);
            await using var compressed = encoded.AsStream();
            compressed.Seek(0, SeekOrigin.Begin);
            await _manager.StoreAsync(target, compressed, StorageKind.Image, cancellationToken);
        }
    }
}
