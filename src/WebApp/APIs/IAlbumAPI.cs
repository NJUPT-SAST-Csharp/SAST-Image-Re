using Refit;
using WebApp.APIs;
using WebApp.APIs.Dtos;

namespace WebApp.Requests;

public interface IAlbumAPI
{
    public const string Base = "albums";

    public static string GetCover(long a) => $"{APIConfigurations.BaseUrl}{Base}/cover?a={a}";

    [Get("/")]
    public Task<List<AlbumItemDto>> GetAlbums(
        [AliasAs("c")] long? categoryId = null,
        [AliasAs("a")] long? authorId = null,
        [AliasAs("t")] string title = null!
    );

    [Get("/{id}")]
    public Task<DetailedAlbum> GetDetail(long id);

    [Get("/{id}/images")]
    public Task<IApiResponse<ImageDto[]>> GetImages(long id);
}

public readonly record struct AlbumItemDto(
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
