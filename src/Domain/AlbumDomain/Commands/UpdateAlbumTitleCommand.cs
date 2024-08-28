using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Services;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands
{
    public sealed record class UpdateAlbumTitleCommand(AlbumId Album, AlbumTitle Title, Actor Actor)
        : IDomainCommand { }

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
            await _checker.CheckAsync(command.Title, cancellationToken);

            var album = await _repository.GetAsync(command.Album, cancellationToken);

            album.UpdateTitle(command);
        }
    }
}
