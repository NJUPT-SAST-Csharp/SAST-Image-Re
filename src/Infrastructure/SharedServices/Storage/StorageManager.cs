using Microsoft.Extensions.Options;

namespace Infrastructure.SharedServices.Storage;

public interface IStorageManager
{
    public Task StoreAsync(
        string filename,
        Stream file,
        StorageKind kind,
        CancellationToken cancellationToken = default
    );

    public Task DeleteAsync(
        string filename,
        StorageKind kind,
        CancellationToken cancellationToken = default
    );

    public Stream? FindFile(string id, StorageKind kind);
}

internal sealed class StorageManager(IOptions<StorageOptions> options) : IStorageManager
{
    private readonly StorageOptions _options = options.Value;

    public Task DeleteAsync(
        string filename,
        StorageKind kind,
        CancellationToken cancellationToken = default
    )
    {
        string path = kind switch
        {
            StorageKind.Cover => _options.CoverPath,
            StorageKind.Image => _options.ImagePath,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };

        string[] files = Directory.GetFiles(path, $"{filename}*.*", SearchOption.TopDirectoryOnly);

        foreach (string file in files)
        {
            File.Delete(file);
        }

        return Task.CompletedTask;
    }

    public Stream? FindFile(string id, StorageKind kind)
    {
        string path = kind switch
        {
            StorageKind.Cover => _options.CoverPath,
            StorageKind.Image => _options.ImagePath,
            StorageKind.Avatar => _options.AvatarPath,
            StorageKind.Header => _options.HeaderPath,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };

        string mask = $"{id}.*";

        string[] files = Directory.GetFiles(path, mask, SearchOption.TopDirectoryOnly);

        if (files.Length == 0)
            return null;

        string filename = files[0];
        var stream = File.OpenRead(filename);
        return stream;
    }

    public async Task StoreAsync(
        string filename,
        Stream file,
        StorageKind kind,
        CancellationToken cancellationToken = default
    )
    {
        string path = GetPath(filename, kind);

        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(Directory.GetParent(path)!.FullName);

        await using var stream = File.Create(path);

        await file.CopyToAsync(stream, cancellationToken);
    }

    private string GetPath(string filename, StorageKind kind)
    {
        string option = kind switch
        {
            StorageKind.Cover => _options.CoverPath,
            StorageKind.Image => _options.ImagePath,
            StorageKind.Avatar => _options.AvatarPath,
            StorageKind.Header => _options.HeaderPath,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
        };

        string path = Path.Combine(option, filename);

        return path;
    }
}
