using Application.Query;
using Domain.Shared;

namespace Application.ImageServices.Queries;

public sealed record class ImagesQuery(long? AuthorId, long? AlbumId, int Page, Actor Actor)
    : IQueryRequest<ImageDto[]> { }

internal sealed class ImagesQueryHandler(IQueryRepository<ImagesQuery, ImageDto[]> repository)
    : IQueryRequestHandler<ImagesQuery, ImageDto[]>
{
    public Task<ImageDto[]> Handle(ImagesQuery request, CancellationToken cancellationToken)
    {
        return repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
