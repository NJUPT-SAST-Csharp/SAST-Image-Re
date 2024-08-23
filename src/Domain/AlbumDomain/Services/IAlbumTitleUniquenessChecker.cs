using Domain.AlbumDomain.AlbumEntity;

namespace Domain.AlbumDomain.Services
{
    public interface IAlbumTitleUniquenessChecker : IDomainService
    {
        public Task CheckAsync(AlbumTitle title, CancellationToken cancellationToken = default);
    }
}
