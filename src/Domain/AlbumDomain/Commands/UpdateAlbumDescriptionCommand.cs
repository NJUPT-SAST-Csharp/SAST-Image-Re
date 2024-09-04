using Domain.AlbumDomain.AlbumEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands
{
    public sealed record class UpdateAlbumDescriptionCommand(
        AlbumId Album,
        AlbumDescription Description,
        Actor Actor
    ) : IDomainCommand { }

    internal sealed class UpdateAlbumDescriptionCommandHandler(
        IRepository<Album, AlbumId> repository
    ) : IDomainCommandHandler<UpdateAlbumDescriptionCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;

        public async Task Handle(
            UpdateAlbumDescriptionCommand request,
            CancellationToken cancellationToken
        )
        {
            var album = await _repository.GetAsync(request.Album, cancellationToken);

            album.UpdateDescription(request);
        }
    }
}
