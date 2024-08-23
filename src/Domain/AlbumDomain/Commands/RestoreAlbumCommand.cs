using Domain.AlbumDomain.AlbumEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands
{
    public readonly record struct RestoreAlbumCommand(AlbumId Album, Actor Actor)
        : IDomainCommand { }

    internal sealed class RestoreAlbumCommandHandler(IRepository<Album, AlbumId> repository)
        : ICommandHandler<RestoreAlbumCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;

        public async Task Handle(RestoreAlbumCommand request, CancellationToken cancellationToken)
        {
            var album = await _repository.GetAsync(request.Album, cancellationToken);

            album.Restore(in request);
        }
    }
}
