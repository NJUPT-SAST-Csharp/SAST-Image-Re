using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands
{
    public sealed record class AddImageCommand(
        AlbumId Album,
        ImageTitle Title,
        ImageTags Tags,
        Stream ImageFile,
        Actor Actor
    ) : IDomainCommand { }

    internal sealed class AddImageCommandHandler(IRepository<Album, AlbumId> repository)
        : ICommandHandler<AddImageCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;

        public async Task Handle(AddImageCommand request, CancellationToken cancellationToken)
        {
            var album = await _repository.GetAsync(request.Album, cancellationToken);

            album.AddImage(request);
        }
    }
}
