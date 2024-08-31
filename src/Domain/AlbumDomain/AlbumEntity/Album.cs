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

        private AlbumStatus _status = AlbumStatus.Available;

        private AlbumTitle _title;

        private Cover _cover = Cover.Default;

        private AccessLevel _accessLevel;

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
                    command.AccessLevel
                )
            );
        }

        public void UpdateDescription(UpdateAlbumDescriptionCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (_status.IsRemoved)
                throw new AlbumRemovedException();

            AddDomainEvent(new AlbumDescriptionUpdatedEvent(Id, command.Description));
        }

        public void UpdateAccessLevel(UpdateAccessLevelCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (_status.IsRemoved)
                throw new AlbumRemovedException();
            if (_accessLevel == command.AccessLevel)
                return;

            _accessLevel = command.AccessLevel;

            AddDomainEvent(new AlbumAccessLevelUpdatedEvent(Id, command.AccessLevel));
        }

        public void UpdateTitle(UpdateAlbumTitleCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (_status.IsRemoved)
                throw new AlbumRemovedException();
            if (_title == command.Title)
                return;

            _title = command.Title;

            AddDomainEvent(new AlbumTitleUpdatedEvent(Id, _title));
        }

        public void UpdateCollaborators(UpdateCollaboratorsCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (_status.IsRemoved)
                throw new AlbumRemovedException();

            if (_collaborators.SequenceEqual(command.Collaborators))
                return;

            Collaborators = command.Collaborators;

            AddDomainEvent(new AlbumCollaboratorsUpdatedEvent(Id, Collaborators));
        }

        public void UpdateCategory(UpdateAlbumCategoryCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (_status.IsRemoved)
                throw new AlbumRemovedException();

            AddDomainEvent(new AlbumCategoryUpdatedEvent(Id, command.Category));
        }

        public void UpdateCover(UpdateCoverCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (_status.IsRemoved)
                throw new AlbumRemovedException();

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

        public void AddImage(AddImageCommand command)
        {
            if (CanNotManageImages(command.Actor))
                NoPermissionException.Throw();
            if (_status.IsRemoved)
                throw new AlbumRemovedException();

            var image = new Image(command);

            _images.Add(image);

            AddDomainEvent(
                new ImageAddedEvent(
                    Id,
                    image.Id,
                    command.Actor.Id,
                    command.Title,
                    command.Tags,
                    _accessLevel,
                    Collaborators,
                    command.ImageFile
                )
            );

            if (_cover.IsLatestImage)
            {
                _cover = _cover with { Id = image.Id };
                AddDomainEvent(AlbumCoverUpdatedEvent.NewAddedImage(Id, command.ImageFile));
            }
        }

        public void RemoveImage(RemoveImageCommand command)
        {
            if (CanNotManageImages(command.Actor))
                NoPermissionException.Throw();
            if (_status.IsRemoved)
                throw new AlbumRemovedException();

            var image = _images.FindById(command.Image);
            image.Remove(command);

            if (_cover.Id == command.Image)
            {
                var imageId = _images.LatestImage()?.Id;
                _cover = _cover with { Id = imageId };
                AddDomainEvent(AlbumCoverUpdatedEvent.ContainedImageOrEmpty(Id, imageId));
            }
        }

        public void RestoreImage(RestoreImageCommand command)
        {
            if (CanNotManageImages(command.Actor))
                NoPermissionException.Throw();
            if (_status.IsRemoved)
                throw new AlbumRemovedException();

            var image = _images.FindById(command.Image);
            image.Restore(command);

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
            if (_status.IsRemoved)
                return;

            _status = AlbumStatus.Removed(DateTime.UtcNow);

            AddDomainEvent(new AlbumRemovedEvent(Id, _status));
            foreach (var image in _images)
            {
                image.AlbumRemoved(command);
            }
        }

        public void Restore(RestoreAlbumCommand command)
        {
            if (CanNotManage(command.Actor))
                NoPermissionException.Throw();
            if (_status.IsAvailable)
                return;

            _status = AlbumStatus.Available;

            AddDomainEvent(new AlbumRestoredEvent(Id, _status));
            foreach (var image in _images)
            {
                image.AlbumRestored(command);
            }
        }

        public void Subscribe(SubscribeCommand command)
        {
            if (_status.IsRemoved)
                throw new AlbumRemovedException();
            if (_subscribes.ContainsUser(command.Actor.Id))
                return;

            _subscribes.Add(new(Id, command.Actor.Id));

            AddDomainEvent(new AlbumSubscribedEvent(Id, command.Actor.Id));
        }

        public void Unsubscribe(UnsubscribeCommand command)
        {
            if (_status.IsRemoved)
                throw new AlbumRemovedException();
            if (_subscribes.NotContainsUser(command.Actor.Id))
                return;

            _subscribes.RemoveUser(command.Actor.Id);

            AddDomainEvent(new AlbumUnsubscribedEvent(Id, command.Actor.Id));
        }

        public void LikeImage(LikeImageCommand command)
        {
            if (_status.IsRemoved)
                throw new AlbumRemovedException();

            var image = _images.FindById(command.Image);

            image.Like(command);
        }

        public void UnlikeImage(UnlikeImageCommand command)
        {
            if (_status.IsRemoved)
                throw new AlbumRemovedException();

            var image = _images.FindById(command.Image);

            image.Unlike(command);
        }

        public void UpdateImageTags(UpdateImageTagsCommand command)
        {
            if (_status.IsRemoved)
                throw new AlbumRemovedException();

            var image = _images.FindById(command.Image);

            image.UpdateTags(command);
        }

        private bool IsOwnedBy(Actor actor) => _author == actor.Id;

        private bool CanManage(Actor actor) => IsOwnedBy(actor) || actor.IsAdmin;

        private bool CanNotManage(Actor actor) => !CanManage(actor);

        private bool CanManageImages(Actor actor) =>
            CanManage(actor) || Collaborators.Contains(actor.Id);

        private bool CanNotManageImages(Actor actor) => !CanManageImages(actor);
    }
}
