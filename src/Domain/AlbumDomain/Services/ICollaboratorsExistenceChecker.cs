using Domain.AlbumDomain.AlbumEntity;

namespace Domain.AlbumDomain.Services
{
    public interface ICollaboratorsExistenceChecker
    {
        public Task CheckAsync(
            Collaborators collaborators,
            CancellationToken cancellationToken = default
        );
    }
}
