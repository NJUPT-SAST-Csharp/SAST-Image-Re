using Domain.AlbumDomain.Exceptions;
using Domain.AlbumDomain.Services;
using SkiaSharp;

namespace Infrastructure.Domain.AlbumServices
{
    internal sealed class ImageFileChecker : IImageFileChecker
    {
        public async Task CheckAsync(Stream file, CancellationToken cancellationToken = default)
        {
            await using var stream = new MemoryStream();
            await file.CopyToAsync(stream, cancellationToken);
            file.Seek(0, SeekOrigin.Begin);
            stream.Seek(0, SeekOrigin.Begin);

            var code = SKCodec.Create(stream);

            if (code is null)
                FileNotImageException.Throw();
        }
    }
}
