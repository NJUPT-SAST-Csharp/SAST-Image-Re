using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Domain.AlbumDomain.Services;
using Domain.Command;
using Domain.Extensions;
using Domain.UserDomain.UserEntity;

namespace Domain.AlbumDomain.Commands
{
    public sealed record class UpdateImageTagsCommand(
        AlbumId Album,
        ImageId Image,
        ImageTags Tags,
        UserId Operator
    ) : IDomainCommand { }

    internal sealed class UpdateImageTagsCommandHandler(
        IRepository<Album, AlbumId> repository,
        IImageTagsExistenceChecker checker
    ) : ICommandHandler<UpdateImageTagsCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;
        private readonly IImageTagsExistenceChecker _checker = checker;

        public async Task Handle(
            UpdateImageTagsCommand request,
            CancellationToken cancellationToken
        )
        {
            await _checker.CheckAsync(request.Tags, cancellationToken);

            var album = await _repository.GetAsync(request.Album, cancellationToken);

            album.UpdateImageTags(request);
        }
    }
}
