using Domain.AlbumDomain.AlbumEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands;

public sealed record class SubscribeCommand(AlbumId Album, Actor Actor) : IDomainCommand { }

internal sealed class SubscribeCommandHandler(IRepository<Album, AlbumId> repository)
    : IDomainCommandHandler<SubscribeCommand>
{
    private readonly IRepository<Album, AlbumId> _repository = repository;

    public async Task Handle(SubscribeCommand request, CancellationToken cancellationToken)
    {
        var album = await _repository.GetAsync(request.Album, cancellationToken);

        album.Subscribe(request);
    }
}
