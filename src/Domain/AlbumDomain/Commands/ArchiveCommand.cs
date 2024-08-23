using Domain.AlbumDomain.AlbumEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands
{
    public readonly record struct ArchiveCommand(AlbumId Album, Actor Actor) : IDomainCommand { }

    internal sealed class ArchiveCommandHandler(IRepository<Album, AlbumId> repository)
        : ICommandHandler<ArchiveCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;

        public async Task Handle(ArchiveCommand request, CancellationToken cancellationToken)
        {
            var album = await _repository.GetAsync(request.Album, cancellationToken);

            album.Archive(in request);
        }
    }
}
