using Domain.AlbumEntity;
using Domain.AlbumEntity.Commands;
using Domain.Command;
using Domain.Service;

namespace Application.AlbumEntity.Commands
{
    internal sealed class UpdateAlbumDescriptionCommandHandler(
        IRepository<Album, AlbumId> repository
    ) : ICommandHandler<UpdateAlbumDescriptionCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;

        public async Task Handle(
            UpdateAlbumDescriptionCommand request,
            CancellationToken cancellationToken
        )
        {
            var album = await _repository.GetAsync(request.Album, cancellationToken);

            album.Handle(request);
        }
    }
}
