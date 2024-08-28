﻿using Microsoft.Extensions.Options;

namespace Infrastructure.Storage
{
    public interface IStorageManager
    {
        public Task StoreAsync(
            string filename,
            Stream file,
            StorageKind kind,
            CancellationToken cancellationToken = default
        );

        public Task DeleteAsync(string path, CancellationToken cancellationToken = default);

        public Stream? FindFile(string id, StorageKind kind);
    }

    internal sealed class StorageManager(IOptions<StorageOptions> options) : IStorageManager
    {
        private readonly StorageOptions _options = options.Value;

        public Task DeleteAsync(string path, CancellationToken cancellationToken = default)
        {
            File.Delete(path);
            return Task.CompletedTask;
        }

        public Stream? FindFile(string id, StorageKind kind)
        {
            string path = kind switch
            {
                StorageKind.Cover => _options.CoverPath,
                StorageKind.Image => _options.ImagePath,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
            };

            string mask = $"{id}.*";

            var files = Directory.GetFiles(path, mask, SearchOption.TopDirectoryOnly);

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
            var path = GetPath(filename, kind);

            await using var stream = File.Create(path);

            await file.CopyToAsync(stream, cancellationToken);
        }

        private string GetPath(string filename, StorageKind kind)
        {
            string option = kind switch
            {
                StorageKind.Cover => _options.CoverPath,
                StorageKind.Image => _options.ImagePath,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
            };

            string path = Path.Combine(option, filename);

            return path;
        }
    }
}