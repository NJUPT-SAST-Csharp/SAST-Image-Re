using Application.Query;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Shared;

namespace Application.AlbumServices.Queries
{
    public sealed class RemovedAlbumDto(AlbumModel a)
    {
        public long Id { get; } = a.Id;
        public string Title { get; } = a.Title;
        public long Category { get; } = a.CategoryId;
        public AccessLevelValue AccessLevel { get; } = a.AccessLevel;
        public DateTime RemovedAt { get; } = a.RemovedAt!.Value;
    }

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
