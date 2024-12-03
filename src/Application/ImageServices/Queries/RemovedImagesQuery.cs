using Application.Query;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Shared;

namespace Application.ImageServices.Queries;

public sealed record RemovedImagesQuery(AlbumId Album, Actor Actor) : IQueryRequest<List<ImageDto>>;

internal sealed class RemovedImagesQueryHandler(
    IQueryRepository<RemovedImagesQuery, List<ImageDto>> repository
) : IQueryRequestHandler<RemovedImagesQuery, List<ImageDto>>
{
    private readonly IQueryRepository<RemovedImagesQuery, List<ImageDto>> _repository = repository;

    public Task<List<ImageDto>> Handle(
        RemovedImagesQuery request,
        CancellationToken cancellationToken
    )
    {
        return _repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
