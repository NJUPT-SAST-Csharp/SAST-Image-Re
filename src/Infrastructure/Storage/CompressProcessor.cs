using SkiaSharp;

namespace Infrastructure.Storage
{
    public interface ICompressProcessor
    {
        public Task CompressAsync(
            string filename,
            int rate,
            CancellationToken cancellationToken = default
        );
    }

    internal sealed class CompressProcessor : ICompressProcessor
    {
        public async Task CompressAsync(
            string filename,
            int rate,
            CancellationToken cancellationToken = default
        )
        {
            string target = Path.ChangeExtension(filename, ".webp");

            using var image = SKBitmap.Decode(filename);
            using var data = image.PeekPixels();
            using var encoded = data.Encode(SKEncodedImageFormat.Webp, 50);
            await using var targetFile = File.OpenWrite(target);
            encoded.SaveTo(targetFile);
        }
    }
}
