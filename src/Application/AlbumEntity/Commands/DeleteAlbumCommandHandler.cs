using Domain.AlbumEntity;
using Domain.AlbumEntity.Commands;
using Domain.Command;
using Domain.Service;

namespace Application.AlbumEntity.Commands
{
    internal sealed class DeleteAlbumCommandHandler(IRepository<Album, AlbumId> repository)
        : ICommandHandler<DeleteAlbumCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;

        public async Task Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
        {
            var album = await _repository.GetAsync(request.Album, cancellationToken);

            await album.Handle(request, _repository, cancellationToken);
        }
    }
}
