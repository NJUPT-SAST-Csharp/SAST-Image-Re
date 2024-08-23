using Application.Query;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Shared;

namespace Application.AlbumServices.Queries
{
    public record class DetailedAlbum(
        long Id,
        string Title,
        string Description,
        long Author,
        long Category,
        bool IsArchived,
        DateTime UpdatedAt,
        DateTime CreatedAt,
        AccessibilityValue Accessibility,
        int SubscribeCount
    ) { }

    public readonly record struct DetailedAlbumQuery(long Id, Actor Actor)
        : IQueryRequest<DetailedAlbum?>;

    internal sealed class DetailedAlbumQueryHandler(
        IQueryRepository<DetailedAlbumQuery, DetailedAlbum?> repository
    ) : IQueryRequestHandler<DetailedAlbumQuery, DetailedAlbum?>
    {
        private readonly IQueryRepository<DetailedAlbumQuery, DetailedAlbum?> _repository =
            repository;

        public Task<DetailedAlbum?> Handle(
            DetailedAlbumQuery request,
            CancellationToken cancellationToken
        )
        {
            return _repository.GetOrDefaultAsync(request, cancellationToken);
        }
    }
}
