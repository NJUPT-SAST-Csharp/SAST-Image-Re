using Application.Query;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Shared;

namespace Application.ImageServices.Queries;

public sealed record RemovedImagesQuery(AlbumId Album, Actor Actor) : IQueryRequest<ImageDto[]>;

internal sealed class RemovedImagesQueryHandler(
    IQueryRepository<RemovedImagesQuery, ImageDto[]> repository
) : IQueryRequestHandler<RemovedImagesQuery, ImageDto[]>
{
    private readonly IQueryRepository<RemovedImagesQuery, ImageDto[]> _repository = repository;

    public Task<ImageDto[]> Handle(RemovedImagesQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
