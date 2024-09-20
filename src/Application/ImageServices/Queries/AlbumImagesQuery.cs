using Application.Query;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Shared;

namespace Application.ImageServices.Queries;

public sealed record AlbumImageDto(long Id, string Title, long[] Tags);

public sealed record AlbumImagesQuery(AlbumId Album, Actor Actor)
    : IQueryRequest<List<AlbumImageDto>> { }

internal sealed class AlbumImagesQueryHandler(
    IQueryRepository<AlbumImagesQuery, List<AlbumImageDto>> repository
) : IQueryRequestHandler<AlbumImagesQuery, List<AlbumImageDto>>
{
    private readonly IQueryRepository<AlbumImagesQuery, List<AlbumImageDto>> _repository =
        repository;

    public Task<List<AlbumImageDto>> Handle(
        AlbumImagesQuery request,
        CancellationToken cancellationToken
    )
    {
        return _repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
