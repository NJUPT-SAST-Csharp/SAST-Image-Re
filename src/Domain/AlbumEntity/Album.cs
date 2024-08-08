using Domain.CategoryEntity;
using Domain.Internal.Entity;
using Domain.UserEntity;

namespace Domain.AlbumEntity
{
    public sealed class Album : EntityBase<AlbumId>
    {
        private Album()
            : base(default) { }

        internal AlbumTitle Title { get; private set; }

        internal CategoryId CategoryId { get; private set; }

        internal UserId AuthorId { get; }

        internal Collaborators Collaborators { get; private set; } = [];

        internal Accessibility Accessibility { get; private set; }

        internal List<AlbumImage> Images { get; } = [];
    }
}
