using Refit;
using WebApp.APIs.Dtos;

namespace WebApp.APIs;

public interface IAlbumAPI
{
    public const string Base = "albums";

    public static string GetCover(long id) => $"{APIConfigurations.BaseUrl}{Base}/{id}/cover";

    [Get("/")]
    public Task<IApiResponse<AlbumDto[]>> GetAlbums(
        [Query] long? category = null,
        [Query] long? author = null,
        [Query] string title = null!
    );

    [Get("/{id}")]
    public Task<IApiResponse<DetailedAlbum>> GetDetail(long id);
}

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
