using Application.Query;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Shared;

namespace Application.AlbumServices.Queries
{
    public sealed record class RemovedAlbumDto(
        long Id,
        string Title,
        long Category,
        AccessibilityValue Accessibility,
        DateTime RemovedAt
    );

    public sealed record class RemovedAlbumsQuery(Actor Actor)
        : IQueryRequest<List<RemovedAlbumDto>> { }

    internal sealed class RemovedAlbumsQueryHandler(
        IQueryRepository<RemovedAlbumsQuery, List<RemovedAlbumDto>> repository
    ) : IQueryRequestHandler<RemovedAlbumsQuery, List<RemovedAlbumDto>>
    {
        private readonly IQueryRepository<RemovedAlbumsQuery, List<RemovedAlbumDto>> _repository =
            repository;

        public Task<List<RemovedAlbumDto>> Handle(
            RemovedAlbumsQuery request,
            CancellationToken cancellationToken
        )
        {
            return _repository.GetOrDefaultAsync(request, cancellationToken);
        }
    }
}
