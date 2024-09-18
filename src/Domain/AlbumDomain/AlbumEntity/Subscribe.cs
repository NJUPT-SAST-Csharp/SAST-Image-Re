using Domain.UserDomain.UserEntity;

namespace Domain.AlbumDomain.AlbumEntity;

public sealed record class Subscribe(AlbumId Album, UserId User) { }

internal static class SubscribeListExtensions
{
    public static bool ContainsUser(this List<Subscribe> subscribes, UserId userId)
    {
        return subscribes.Any(subscribe => subscribe.User == userId);
    }

    public static bool NotContainsUser(this List<Subscribe> subscribes, UserId userId)
    {
        return subscribes.ContainsUser(userId) == false;
    }

    public static bool RemoveUser(this List<Subscribe> subscribes, UserId userId)
    {
        return subscribes.RemoveAll(subscribe => subscribe.User == userId) > 0;
    }
}
