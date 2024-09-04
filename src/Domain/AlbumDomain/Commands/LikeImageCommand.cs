using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands
{
    public sealed record class LikeImageCommand(AlbumId Album, ImageId Image, Actor Actor)
        : IDomainCommand { }

    internal sealed class LikeCommandHandler(IRepository<Album, AlbumId> repository)
        : IDomainCommandHandler<LikeImageCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;

        public async Task Handle(LikeImageCommand request, CancellationToken cancellationToken)
        {
            var album = await _repository.GetAsync(request.Album);

            album.LikeImage(request);
        }
    }
}
