namespace Infrastructure.Storage
{
    public interface IStorageManager
    {
        public Task StoreAsync(
            string path,
            Stream file,
            CancellationToken cancellationToken = default
        );

        public Task DeleteAsync(string path, CancellationToken cancellationToken = default);
    }

    internal class StorageManager : IStorageManager
    {
        public Task DeleteAsync(string path, CancellationToken cancellationToken = default)
        {
            File.Delete(path);
            return Task.CompletedTask;
        }

        public async Task StoreAsync(
            string path,
            Stream file,
            CancellationToken cancellationToken = default
        )
        {
            await using var stream = File.Create(path);

            await file.CopyToAsync(stream, cancellationToken);
        }
    }
}
