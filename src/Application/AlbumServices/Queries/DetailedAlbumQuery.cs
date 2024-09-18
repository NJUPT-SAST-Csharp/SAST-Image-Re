using Application.Query;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Shared;

namespace Application.AlbumServices.Queries;

public sealed class DetailedAlbum(AlbumModel album, int subscribeCount)
{
    public long Id { get; } = album.Id;
    public string Title { get; } = album.Title;
    public string Description { get; } = album.Description;
    public long Author { get; } = album.AuthorId;
    public long Category { get; } = album.CategoryId;
    public DateTime UpdatedAt { get; } = album.UpdatedAt;
    public DateTime CreatedAt { get; } = album.CreatedAt;
    public AccessLevelValue AccessLevel { get; } = album.AccessLevel;
    public int SubscribeCount { get; } = subscribeCount;
}

public sealed record class DetailedAlbumQuery(long Id, Actor Actor) : IQueryRequest<DetailedAlbum?>;

internal sealed class DetailedAlbumQueryHandler(
    IQueryRepository<DetailedAlbumQuery, DetailedAlbum?> repository
) : IQueryRequestHandler<DetailedAlbumQuery, DetailedAlbum?>
{
    private readonly IQueryRepository<DetailedAlbumQuery, DetailedAlbum?> _repository = repository;

    public Task<DetailedAlbum?> Handle(
        DetailedAlbumQuery request,
        CancellationToken cancellationToken
    )
    {
        return _repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
