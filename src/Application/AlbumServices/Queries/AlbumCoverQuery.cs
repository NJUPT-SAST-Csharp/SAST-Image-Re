using Application.Query;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Shared;

namespace Application.AlbumServices.Queries
{
    public sealed record class AlbumCoverQuery(long Id, Actor Actor) : IQueryRequest<Stream?>;

    internal sealed class AlbumCoverQueryHandler(
        IAlbumCoverManager manager,
        IAlbumAvailabilityChecker checker
    ) : IQueryRequestHandler<AlbumCoverQuery, Stream?>
    {
        private readonly IAlbumCoverManager _manager = manager;
        private readonly IAlbumAvailabilityChecker _checker = checker;

        public async Task<Stream?> Handle(
            AlbumCoverQuery request,
            CancellationToken cancellationToken
        )
        {
            AlbumId id = new(request.Id);

            bool available = await _checker.CheckAsync(id, request.Actor, cancellationToken);

            if (available == false)
                return null;

            var stream = _manager.OpenReadStream(id);

            return stream;
        }
    }
}
