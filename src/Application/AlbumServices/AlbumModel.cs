using Application.ImageServices;
using Domain.AlbumDomain.AlbumEntity;

namespace Application.AlbumServices
{
    public sealed class AlbumModel
    {
        public required long Id { get; init; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required long AuthorId { get; set; }
        public required long CategoryId { get; set; }
        public required AccessibilityValue Accessibility { get; set; }
        public long[] Collaborators { get; set; } = [];
        public bool IsArchived { get; set; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RemovedAt { get; set; } = null;
        public List<SubscribeModel> Subscribes { get; } = [];
        public List<ImageModel> Images { get; } = [];
    }

    public sealed record class SubscribeModel(long Album, long User);
}
