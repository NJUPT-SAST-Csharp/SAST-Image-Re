using Application.Query;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Shared;

namespace Application.ImageServices.Queries
{
    public sealed record RemovedImageDto(long Id, string Title);

    public sealed record RemovedImagesQuery(AlbumId Album, Actor Actor)
        : IQueryRequest<List<RemovedImageDto>>;

    internal sealed class RemovedImagesQueryHandler(
        IQueryRepository<RemovedImagesQuery, List<RemovedImageDto>> repository
    ) : IQueryRequestHandler<RemovedImagesQuery, List<RemovedImageDto>>
    {
        private readonly IQueryRepository<RemovedImagesQuery, List<RemovedImageDto>> _repository =
            repository;

        public Task<List<RemovedImageDto>> Handle(
            RemovedImagesQuery request,
            CancellationToken cancellationToken
        )
        {
            return _repository.GetOrDefaultAsync(request, cancellationToken);
        }
    }
}
