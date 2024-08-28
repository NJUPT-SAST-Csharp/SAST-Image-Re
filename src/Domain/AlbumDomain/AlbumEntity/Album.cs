using Domain.AlbumDomain.Commands;
using Domain.AlbumDomain.Events;
using Domain.AlbumDomain.Exceptions;
using Domain.AlbumDomain.ImageEntity;
using Domain.Entity;
using Domain.Shared;
using Domain.UserDomain.UserEntity;

namespace Domain.AlbumDomain.AlbumEntity
{
    public sealed class Album : EntityBase<AlbumId>
    {
        private Album()
            : base(default) { }

        private bool _isRemoved;

        private bool _isArchived;

        private AlbumTitle _title;

        private Cover _cover = Cover.Default;

        private readonly UserId _author;

        private readonly List<Subscribe> _subscribes = [];

        private readonly List<Image> _images = [];

        private UserId[] _collaborators = [];
        private Collaborators Collaborators
        {
            get => new(_collaborators);
            set => _collaborators = [.. value];
        }

        public Album(CreateAlbumCommand command)
            : base(AlbumId.GenerateNew())
        {
            _author = command.Actor.Id;
            _title = command.Title;

            AddDomainEvent(
                new AlbumCreatedEvent(
                    Id,
                    command.Actor.Id,
                    command.CategoryId,
                    command.Title,
                    command.Description,
                    command.Accessibility
                )
            );
        }

        public void UpdateDescription(ref readonly UpdateAlbumDescriptionCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (IsImmutable)
                AlbumImmutableException.Throw(this);

            AddDomainEvent(new AlbumDescriptionUpdatedEvent(Id, command.Description));
        }

        public void UpdateAccessibility(UpdateAccessibilityCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (_isRemoved)
                AlbumImmutableException.Throw(this);

            AddDomainEvent(new AlbumAccessibilityUpdatedEvent(Id, command.Accessibility));
        }

        public void UpdateTitle(UpdateAlbumTitleCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (IsImmutable)
                AlbumImmutableException.Throw(this);

            _title = command.Title;

            AddDomainEvent(new AlbumTitleUpdatedEvent(Id, _title));
        }

        public void UpdateCollaborators(ref readonly UpdateCollaboratorsCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (IsImmutable)
                AlbumImmutableException.Throw(this);

            if (_collaborators.SequenceEqual(command.Collaborators))
                return;

            Collaborators = command.Collaborators;

            AddDomainEvent(new AlbumCollaboratorsUpdatedEvent(Id, Collaborators));
        }

        public void UpdateCategory(ref readonly UpdateAlbumCategoryCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (IsImmutable)
                AlbumImmutableException.Throw(this);

            AddDomainEvent(new AlbumCategoryUpdatedEvent(Id, command.Category));
        }

        public void UpdateCover(UpdateCoverCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (IsImmutable)
                AlbumImmutableException.Throw(this);

            if (command.CoverImage is null)
            {
                var imageId = _images.LatestImage()?.Id;
                _cover = new(imageId, true);
                AddDomainEvent(AlbumCoverUpdatedEvent.ContainedImageOrEmpty(Id, imageId));
                return;
            }

            _cover = Cover.UserCustomCover;
            AddDomainEvent(AlbumCoverUpdatedEvent.UserCustomImage(Id, command.CoverImage));
        }

        public void AddImage(ref readonly AddImageCommand command)
        {
            if (CanNotManageImages(command.Actor))
                NoPermissionException.Throw();
            if (IsImmutable)
                AlbumImmutableException.Throw(this);

            var image = new Image(in command);

            _images.Add(image);

            AddDomainEvent(
                new ImageAddedEvent(
                    Id,
                    image.Id,
                    command.Actor.Id,
                    command.Title,
                    command.Tags,
                    command.ImageFile
                )
            );

            if (_cover.IsLatestImage)
            {
                _cover = _cover with { Id = image.Id };
                AddDomainEvent(AlbumCoverUpdatedEvent.NewAddedImage(Id, command.ImageFile));
            }
        }

        public void RemoveImage(ref readonly RemoveImageCommand command)
        {
            if (CanNotManageImages(command.Actor))
                NoPermissionException.Throw();
            if (IsImmutable)
                AlbumImmutableException.Throw(this);

            var image = _images.FindById(command.Image);
            image.Remove(in command);

            if (_cover.Id == command.Image)
            {
                var imageId = _images.LatestImage()?.Id;
                _cover = _cover with { Id = imageId };
                AddDomainEvent(AlbumCoverUpdatedEvent.ContainedImageOrEmpty(Id, imageId));
            }
        }

        public void RestoreImage(ref readonly RestoreImageCommand command)
        {
            if (CanNotManageImages(command.Actor))
                NoPermissionException.Throw();
            if (IsImmutable)
                AlbumImmutableException.Throw(this);

            var image = _images.FindById(command.Image);
            image.Restore(in command);

            if (_cover.IsLatestImage && image.Equals(_images.LatestImage()))
            {
                _cover = new(image.Id, true);
                AddDomainEvent(AlbumCoverUpdatedEvent.ContainedImageOrEmpty(Id, image.Id));
            }
        }

        public void Remove(RemoveAlbumCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (_isRemoved)
                return;

            _isRemoved = true;

            AddDomainEvent(new AlbumRemovedEvent(Id));
            foreach (var image in _images)
            {
                RemoveImageCommand c = new(Id, image.Id, command.Actor);
                image.Remove(in c);
            }
        }

        public void Restore(RestoreAlbumCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (_isRemoved == false)
                return;

            _isRemoved = false;

            AddDomainEvent(new AlbumRestoredEvent(Id));
            foreach (var image in _images)
            {
                RestoreImageCommand c = new(Id, image.Id, command.Actor);
                image.Restore(in c);
            }
        }

        public void Archive(ArchiveCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (IsImmutable)
                AlbumImmutableException.Throw(this);
            if (_isArchived == true)
                return;

            _isArchived = true;
            Collaborators = [];

            AddDomainEvent(new AlbumArchivedEvent(Id));
            AddDomainEvent(new AlbumCollaboratorsUpdatedEvent(Id, Collaborators));
        }

        public void Subscribe(ref readonly SubscribeAlbumCommand command)
        {
            if (_subscribes.ContainsUser(command.Actor.Id))
                return;

            _subscribes.Add(new(Id, command.Actor.Id));

            AddDomainEvent(new AlbumSubscribedEvent(Id, command.Actor.Id));
        }

        public void Unsubscribe(ref readonly UnsubscribeCommand command)
        {
            if (_subscribes.NotContainsUser(command.Actor.Id))
                return;

            _subscribes.RemoveUser(command.Actor.Id);

            AddDomainEvent(new AlbumUnsubscribedEvent(Id, command.Actor.Id));
        }

        public void LikeImage(ref readonly LikeImageCommand command)
        {
            var image = _images.FindById(command.Image);

            image.Like(in command);
        }

        public void UnlikeImage(ref readonly UnlikeImageCommand command)
        {
            var image = _images.FindById(command.Image);

            image.Unlike(in command);
        }

        public void UpdateImageTags(ref readonly UpdateImageTagsCommand command)
        {
            var image = _images.FindById(command.Image);

            image.UpdateTags(in command);
        }

        private bool IsOwnedBy(Actor actor) => _author == actor.Id;

        private bool CanManage(Actor actor) => IsOwnedBy(actor) || actor.IsAdmin;

        private bool CanNotManage(Actor actor) => !CanManage(actor);

        private bool CanManageImages(Actor actor) =>
            CanManage(actor) || Collaborators.Contains(actor.Id);

        private bool CanNotManageImages(Actor actor) => !CanManageImages(actor);

        private bool IsImmutable => _isRemoved || _isArchived;
        private bool IsMutable => !IsImmutable;
    }
}
