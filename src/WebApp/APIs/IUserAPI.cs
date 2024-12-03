using System;
using Refit;
using WebApp.APIs.Dtos;

namespace WebApp.APIs;

public interface IUserAPI
{
    public const string Base = "users";

    public static string GetAvatar(long id) => $"{APIConfigurations.BaseUrl}{Base}/avatar/{id}";
    public static string GetHeader(long id) => $"{APIConfigurations.BaseUrl}{Base}/header/{id}";

    [Multipart]
    [Post("/avatar")]
    public Task<IApiResponse> UpdateAvatar(StreamPart avatar);

    [Multipart]
    [Post("/header")]
    public Task<IApiResponse> UpdateHeader(StreamPart header);

    [Get("/profile/{id}")]
    public Task<ApiResponse<UserProfileDto>> GetProfile(long id);

    [Get("/{userId}/images")]
    public Task<IApiResponse<ImageDto[]>> GetUserImages(long userId);
}

public readonly record struct UserProfileDto(long Id, string Username, string Biography);
