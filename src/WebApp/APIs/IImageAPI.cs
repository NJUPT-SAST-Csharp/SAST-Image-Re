using Refit;

namespace WebApp.APIs;

public interface IImageAPI
{
    public const string Base = "images";

    [Get("/{id}/file")]
    public Task<IApiResponse<Stream>> GetFile(long id);
}
