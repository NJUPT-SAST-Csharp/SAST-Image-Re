using Refit;

namespace WebApp.APIs;

public interface IImageAPI
{
    [Get("/{id}/file")]
    public Task<IApiResponse<Stream>> GetFile(long id);
}
