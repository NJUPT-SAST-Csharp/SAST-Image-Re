namespace Domain.AlbumDomain.Services
{
    public interface IImageFileChecker
    {
        public Task CheckAsync(Stream file, CancellationToken cancellationToken = default);
    }
}
