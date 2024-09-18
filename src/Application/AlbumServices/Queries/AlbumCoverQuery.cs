using Application.Query;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Shared;

namespace Application.AlbumServices.Queries;

public sealed record class AlbumCoverQuery(AlbumId Id, Actor Actor) : IQueryRequest<Stream?>;

internal sealed class AlbumCoverQueryHandler(
    ICoverStorageManager manager,
    IAlbumAvailabilityChecker checker
) : IQueryRequestHandler<AlbumCoverQuery, Stream?>
{
    private readonly ICoverStorageManager _manager = manager;
    private readonly IAlbumAvailabilityChecker _checker = checker;

    public async Task<Stream?> Handle(AlbumCoverQuery request, CancellationToken cancellationToken)
    {
        bool available = await _checker.CheckAsync(request.Id, request.Actor, cancellationToken);

        if (available == false)
            return null;

        var stream = _manager.OpenReadStream(request.Id);

        return stream;
    }
}
