using System;
using Refit;

namespace WebApp.APIs;

public interface IUserAPI
{
    public const string Base = "users";

    public static string GetAvatar(long id) => $"{APIConfigurations.BaseUrl}{Base}/avatar/{id}";

    [Multipart]
    [Post("/avatar")]
    public Task<IApiResponse> UploadAvatar(StreamPart avatar);
}
