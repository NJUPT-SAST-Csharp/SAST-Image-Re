using Domain.AlbumDomain.AlbumEntity;

namespace Application.ImageServices
{
    public sealed class ImageModel
    {
        public required long Id { get; init; }
        public required string Title { get; set; }
        public required long AlbumId { get; set; }
        public required long AuthorId { get; set; }
        public required long[] Tags { get; set; } = [];
        public bool IsRemoved { get; set; } = false;
        public DateTime UploadedAt { get; init; } = DateTime.UtcNow;
        public AccessibilityValue Accessibility { get; set; }
        public List<LikeModel> Likes { get; } = [];
    }

    public sealed record class LikeModel(long Image, long User);
}
