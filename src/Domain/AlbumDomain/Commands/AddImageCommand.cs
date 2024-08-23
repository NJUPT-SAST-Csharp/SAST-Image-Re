using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Domain.AlbumDomain.Services;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands
{
    public readonly record struct AddImageCommand(
        AlbumId Album,
        ImageTitle Title,
        ImageTags Tags,
        Stream ImageFile,
        Actor Actor
    ) : IDomainCommand { }

    internal sealed class AddImageCommandHandler(
        IRepository<Album, AlbumId> repository,
        IImageTagsExistenceChecker checker
    ) : ICommandHandler<AddImageCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;
        private readonly IImageTagsExistenceChecker _checker = checker;

        public async Task Handle(AddImageCommand request, CancellationToken cancellationToken)
        {
            await _checker.CheckAsync(request.Tags, cancellationToken);

            var album = await _repository.GetAsync(request.Album, cancellationToken);

            album.AddImage(in request);
        }
    }
}
