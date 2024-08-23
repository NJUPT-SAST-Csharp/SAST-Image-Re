using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands
{
    public readonly record struct LikeImageCommand(AlbumId Album, ImageId Image, Actor Actor)
        : IDomainCommand { }

    internal sealed class LikeCommandHandler(IRepository<Album, AlbumId> repository)
        : ICommandHandler<LikeImageCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;

        public async Task Handle(LikeImageCommand request, CancellationToken cancellationToken)
        {
            var album = await _repository.GetAsync(request.Album);
        }
    }
}
