using Application.AlbumServices;
using Application.ImageServices;

namespace Application.UserServices
{
    public sealed class UserModel
    {
        public required long Id { get; init; }
        public required string Username { get; set; }
        public string Biography { get; set; } = string.Empty;
        public Uri? AvatarUrl { get; set; } = null;
        public Uri? HeaderUrl { get; set; } = null;

        public List<SubscribeModel> Subscribes { get; } = [];
        public List<LikeModel> Likes { get; } = [];
    }
}
