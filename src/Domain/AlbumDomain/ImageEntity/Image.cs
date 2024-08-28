using Domain.AlbumDomain.Commands;
using Domain.AlbumDomain.Events;
using Domain.AlbumDomain.Exceptions;
using Domain.Entity;

namespace Domain.AlbumDomain.ImageEntity
{
    internal sealed class Image : EntityBase<ImageId>
    {
        private Image()
            : base(default) { }

        private List<Like> _likes = [];
        private bool _isRemoved = false;
        internal bool IsAvailable => !_isRemoved;

        public Image(AddImageCommand command)
            : base(ImageId.GenerateNew()) { }

        public void Like(LikeImageCommand command)
        {
            if (_likes.ContainsUser(command.Actor.Id))
                return;

            _likes.Add(new(Id, command.Actor.Id));

            AddDomainEvent(new ImageLikedEvent(Id, command.Actor.Id));
        }

        public void Unlike(UnlikeImageCommand command)
        {
            if (_likes.NotContainsUser(command.Actor.Id))
                return;

            _likes.RemoveUser(command.Actor.Id);

            AddDomainEvent(new ImageUnlikedEvent(Id, command.Actor.Id));
        }

        public void UpdateTags(UpdateImageTagsCommand command)
        {
            AddDomainEvent(new ImageTagsUpdatedEvent(Id, command.Tags));
        }

        public void Remove()
        {
            if (_isRemoved == false)
            {
                _isRemoved = true;
                AddDomainEvent(new ImageRemovedEvent(Id));
            }
        }

        public void Restore()
        {
            if (_isRemoved == true)
            {
                _isRemoved = false;
                AddDomainEvent(new ImageRestoredEvent(Id));
            }
        }
    }

    internal static class ImageListExtensions
    {
        public static Image FindById(this List<Image> images, ImageId id)
        {
            var image = images.FirstOrDefault(image => image.Id == id);
            if (image is null)
                ImageNotFoundException.Throw(id);

            return image;
        }

        public static bool Contains(this List<Image> images, ImageId id)
        {
            return images.Any(image => image.Id == id);
        }

        public static bool NotContains(this List<Image> images, ImageId id)
        {
            return !images.Contains(id);
        }

        public static Image? LatestImage(this List<Image> images)
        {
            return images
                .Where(image => image.IsAvailable)
                .OrderByDescending(image => image.Id.Value)
                .FirstOrDefault();
        }
    }
}
