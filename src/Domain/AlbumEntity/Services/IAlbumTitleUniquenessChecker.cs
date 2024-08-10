using Domain.Service;

namespace Domain.AlbumEntity.Services
{
    public interface IAlbumTitleUniquenessChecker : IDomainService
    {
        public Task<bool> IsAlbumTitleUnique(
            AlbumTitle title,
            CancellationToken cancellationToken = default
        );
    }
}
