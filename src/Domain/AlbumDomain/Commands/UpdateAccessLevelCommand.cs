using Domain.AlbumDomain.AlbumEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands;

public sealed record class UpdateAccessLevelCommand(
    AlbumId Album,
    AccessLevel AccessLevel,
    Actor Actor
) : IDomainCommand { }

internal sealed class UpdateAccessLevelCommandHandler(IRepository<Album, AlbumId> repository)
    : IDomainCommandHandler<UpdateAccessLevelCommand>
{
    private readonly IRepository<Album, AlbumId> _repository = repository;

    public async Task Handle(UpdateAccessLevelCommand request, CancellationToken cancellationToken)
    {
        var album = await _repository.GetAsync(request.Album, cancellationToken);

        album.UpdateAccessLevel(request);
    }
}
