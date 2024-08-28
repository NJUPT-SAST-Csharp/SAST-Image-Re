using Domain.AlbumDomain.AlbumEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands
{
    public sealed record class UpdateCoverCommand(AlbumId Album, Stream? CoverImage, Actor Actor)
        : IDomainCommand { }

    internal sealed class UpdateCoverCommandHandler(IRepository<Album, AlbumId> repository)
        : ICommandHandler<UpdateCoverCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;

        public async Task Handle(UpdateCoverCommand request, CancellationToken cancellationToken)
        {
            var album = await _repository.GetAsync(request.Album, cancellationToken);

            album.UpdateCover(request);
        }
    }
}
