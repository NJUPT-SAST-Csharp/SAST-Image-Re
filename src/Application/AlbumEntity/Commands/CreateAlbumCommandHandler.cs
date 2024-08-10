using Domain.AlbumEntity;
using Domain.AlbumEntity.Commands;
using Domain.AlbumEntity.Services;
using Domain.Command;
using Domain.Extensions;
using Domain.Service;

namespace Application.AlbumEntity.Commands
{
    internal sealed class CreateAlbumCommandReceiver(
        IRepository<Album, AlbumId> repository,
        IAlbumTitleUniquenessChecker checker
    ) : ICommandHandler<CreateAlbumCommand, Result<AlbumId>>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;
        private readonly IAlbumTitleUniquenessChecker _checker = checker;

        public async Task<Result<AlbumId>> Handle(
            CreateAlbumCommand command,
            CancellationToken cancellationToken
        )
        {
            var id = await Album.Handle(command, _repository, _checker, cancellationToken);

            return id;
        }
    }
}
