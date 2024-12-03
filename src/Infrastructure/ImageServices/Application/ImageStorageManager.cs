using System.Globalization;
using Application.ImageServices;
using Domain.AlbumDomain.ImageEntity;
using IdGen;
using Infrastructure.SharedServices.Storage;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace Infrastructure.ImageServices.Application;

internal sealed class ImageStorageManager(
    ICompressProcessor compressor,
    IOptions<StorageOptions> options
) : IImageStorageManager
{
    private readonly StorageOptions options = options.Value;

    const int CompressRate = 60;

    public Task DeleteImageAsync(ImageId image, CancellationToken cancellationToken = default)
    {
        string path = Path.Combine(options.ImagePath, image.GetRelativePath());

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }

        return Task.CompletedTask;
    }

    public Stream? OpenReadStream(ImageId image, ImageKind kind)
    {
        string path = Path.Combine(options.ImagePath, image.GetRelativePath());

        if (Directory.Exists(path) == false)
            return null;

        if (ImageKind.Thumbnail == kind)
            return File.OpenRead(Path.Combine(path, "compressed.webp"));

        string? file = Directory
            .GetFiles(path, "original.*", SearchOption.TopDirectoryOnly)
            .FirstOrDefault();

        return file == null ? null : File.OpenRead(file);
    }

    public async Task StoreImageAsync(
        ImageId imageId,
        Stream imageFile,
        CancellationToken cancellationToken = default
    )
    {
        string path = Path.Combine(options.ImagePath, imageId.GetRelativePath());

        using var code = SKCodec.Create(imageFile);
        string format = code.EncodedFormat.ToString().ToLowerInvariant();
        imageFile.Seek(0, SeekOrigin.Begin);

        string original = Path.Combine(path, Path.ChangeExtension("original.xxx", format));
        string compressed = Path.Combine(path, "compressed.webp");

        await using var compressedImageFile = await compressor.CompressAsync(
            imageFile,
            CompressRate,
            cancellationToken
        );

        EnsureDirectoryExists(path);

        await Task.WhenAll(
            StoreFileAsync(imageFile, original, cancellationToken),
            StoreFileAsync(compressedImageFile, compressed, cancellationToken)
        );
    }

    private static async Task StoreFileAsync(
        Stream file,
        string filename,
        CancellationToken cancellationToken
    )
    {
        file.Seek(0, SeekOrigin.Begin);

        await using var fileStream = File.Create(filename);

        await fileStream.CopyToAsync(file, cancellationToken);
    }

    private static void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}

file static class SnowflakeIdExtensions
{
    public static string GetRelativePath(this ImageId id)
    {
        var epoch = IdGeneratorOptions.DefaultEpoch;
        long timestamp = (id.Value >> 22);
        var dateTime = epoch.AddMilliseconds(timestamp);

        return dateTime.ToString($"yyyy/MM/dd/{id.Value}", CultureInfo.InvariantCulture);
    }
}
