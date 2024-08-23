using Domain.AlbumDomain.ImageEntity;

namespace Domain.AlbumDomain.Services
{
    public interface IImageTagsExistenceChecker
    {
        public Task CheckAsync(ImageTags tags, CancellationToken cancellationToken = default);
    }
}
