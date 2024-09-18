using Refit;

namespace WebApp.Requests;

public interface IAlbumAPI
{
    [Get("/")]
    [Headers("Authorization: Bearer")]
    public Task<List<AlbumDto>> GetAlbums(
        [AliasAs("c")] long? categoryId = null,
        [AliasAs("a")] long? authorId = null,
        [AliasAs("t")] string title = null!
    );

    [Get("/{id}")]
    [Headers("Authorization: Bearer")]
    public Task<DetailedAlbum> GetDetail(long id);

    [Get("/{id}/cover")]
    [Headers("Authorization: Bearer")]
    public Task<Stream> GetCover(long id);
}

public readonly record struct AlbumDto(
    long Id,
    string Title,
    long Author,
    long Category,
    DateTime UpdatedAt
);

public readonly record struct DetailedAlbum(
    long Id,
    string Title,
    string Description,
    long Author,
    long Category,
    DateTime UpdatedAt,
    DateTime CreatedAt,
    int SubscribeCount
);
