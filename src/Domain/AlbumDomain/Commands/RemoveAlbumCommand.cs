using Domain.AlbumDomain.AlbumEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands
{
    public readonly record struct RemoveAlbumCommand(AlbumId Album, Actor Actor)
        : IDomainCommand { }

    internal sealed class RemoveAlbumCommandHandler(IRepository<Album, AlbumId> repository)
        : ICommandHandler<RemoveAlbumCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;

        public async Task Handle(RemoveAlbumCommand request, CancellationToken cancellationToken)
        {
            var album = await _repository.GetAsync(request.Album, cancellationToken);

            album.Remove(in request);
        }
    }
}
