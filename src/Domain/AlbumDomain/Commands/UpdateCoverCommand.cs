using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Services;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands
{
    public sealed record class UpdateCoverCommand(AlbumId Album, Stream? CoverImage, Actor Actor)
        : IDomainCommand { }

    internal sealed class UpdateCoverCommandHandler(
        IRepository<Album, AlbumId> repository,
        IImageFileChecker checker
    ) : ICommandHandler<UpdateCoverCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;
        private readonly IImageFileChecker _checker = checker;

        public async Task Handle(UpdateCoverCommand request, CancellationToken cancellationToken)
        {
            if (request.CoverImage is not null)
                await _checker.CheckAsync(request.CoverImage, cancellationToken);

            var album = await _repository.GetAsync(request.Album, cancellationToken);

            album.UpdateCover(request);
        }
    }
}
