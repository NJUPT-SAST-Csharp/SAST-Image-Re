using System;
using Refit;
using WebApp.APIs.Dtos;

namespace WebApp.APIs;

public interface IUserAPI
{
    public const string Base = "users";

    public static string GetAvatar(long id) => $"{APIConfigurations.BaseUrl}{Base}/{id}/avatar";
    public static string GetHeader(long id) => $"{APIConfigurations.BaseUrl}{Base}/{id}/header";

    [Multipart]
    [Post("/avatar")]
    public Task<IApiResponse> UpdateAvatar(StreamPart avatar);

    [Multipart]
    [Post("/header")]
    public Task<IApiResponse> UpdateHeader(StreamPart header);

    [Get("/{id}/profile")]
    public Task<ApiResponse<UserProfileDto>> GetProfile(long id);
}

public readonly record struct UserProfileDto(long Id, string Username, string Biography);
