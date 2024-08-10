using Domain.AlbumEntity;
using Domain.AlbumEntity.Commands;
using Domain.AlbumEntity.Services;
using Domain.Command;
using Domain.Service;

namespace Application.AlbumEntity.Commands
{
    internal sealed class UpdateAlbumTitleCommandHandler(
        IRepository<Album, AlbumId> repository,
        IAlbumTitleUniquenessChecker checker
    ) : ICommandHandler<UpdateAlbumTitleCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;
        private readonly IAlbumTitleUniquenessChecker _checker = checker;

        public async Task Handle(
            UpdateAlbumTitleCommand command,
            CancellationToken cancellationToken = default
        )
        {
            var album = await _repository.GetAsync(command.Album, cancellationToken);

            await album.Handle(command, _checker, cancellationToken);
        }
    }
}
