using Application.Query;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Shared;

namespace Application.ImageServices.Queries;

public sealed record AlbumImagesQuery(AlbumId Album, Actor Actor)
    : IQueryRequest<List<ImageDto>> { }

internal sealed class AlbumImagesQueryHandler(
    IQueryRepository<AlbumImagesQuery, List<ImageDto>> repository
) : IQueryRequestHandler<AlbumImagesQuery, List<ImageDto>>
{
    private readonly IQueryRepository<AlbumImagesQuery, List<ImageDto>> _repository = repository;

    public Task<List<ImageDto>> Handle(
        AlbumImagesQuery request,
        CancellationToken cancellationToken
    )
    {
        return _repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
