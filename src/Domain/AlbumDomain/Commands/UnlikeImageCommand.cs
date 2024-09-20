using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands;

public sealed record class UnlikeImageCommand(AlbumId Album, ImageId Image, Actor Actor)
    : IDomainCommand { }

internal sealed class UnlikeCommandHandler(IRepository<Album, AlbumId> repository)
    : IDomainCommandHandler<UnlikeImageCommand>
{
    private readonly IRepository<Album, AlbumId> _repository = repository;

    public async Task Handle(UnlikeImageCommand request, CancellationToken cancellationToken)
    {
        var album = await _repository.GetAsync(request.Album, cancellationToken);

        album.UnlikeImage(request);
    }
}
