using SkiaSharp;

namespace Infrastructure.SharedServices.Storage;

public interface ICompressProcessor
{
    public Task<Stream> CompressAsync(
        Stream originalFile,
        int rate,
        CancellationToken cancellationToken = default
    );
}

internal sealed class CompressProcessor : ICompressProcessor
{
    public Task<Stream> CompressAsync(
        Stream originalFile,
        int rate,
        CancellationToken cancellationToken = default
    )
    {
        return Task.Run(
            () =>
            {
                originalFile.Seek(0, SeekOrigin.Begin);
                using var image = SKBitmap.Decode(originalFile);
                using var data = image.PeekPixels();
                var stream = data.Encode(SKEncodedImageFormat.Webp, rate).AsStream();
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            },
            cancellationToken
        );
    }
}
