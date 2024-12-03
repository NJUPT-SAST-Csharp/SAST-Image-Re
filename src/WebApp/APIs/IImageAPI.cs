using Refit;
using WebApp.APIs.Dtos;

namespace WebApp.APIs;

public interface IImageAPI
{
    public const string Base = "images";

    public static string GetImage(long i) => $"{APIConfigurations.BaseUrl}{Base}/{i}";

    [Get("/")]
    public Task<IApiResponse<ImageDto[]>> GetImages(
        [Query] long? uploader = null,
        [Query] long? album = null,
        [Query] int page = 0
    );
}
