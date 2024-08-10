using System.Diagnostics.CodeAnalysis;
using Domain.AlbumEntity.Commands;
using Domain.AlbumEntity.Services;
using Domain.CategoryEntity;
using Domain.Entity;
using Domain.Extensions;
using Domain.ImageEntity;
using Domain.Service;
using Domain.UserEntity;

namespace Domain.AlbumEntity
{
    public sealed class Album : EntityBase<AlbumId>
    {
        private Album()
            : base(AlbumId.GenerateNew()) { }

        public static bool TryCreateNew(
            CreateAlbumCommand command,
            [NotNullWhen(true)] out Album? album
        )
        {
            album = new()
            {
                Title = command.Title,
                Description = command.Description,
                CategoryId = command.CategoryId,
                AuthorId = command.AuthorId,
                Accessibility = command.Accessibility
            };

            return true;
        }

        internal AlbumTitle Title { get; private set; }

        internal AlbumDescription Description { get; private set; }

        internal CategoryId CategoryId { get; private set; }

        internal UserId AuthorId { get; private init; }

        internal Collaborators Collaborators { get; private set; } = [];

        internal Accessibility Accessibility { get; private set; }

        internal List<Image> Images { get; } = [];

        public void Handle(UpdateAccessibilityCommand command)
        {
            if (AuthorId == command.Operator)
            {
                Accessibility = command.Accessibility;
            }
        }

        public async Task Handle(
            UpdateAlbumTitleCommand command,
            IAlbumTitleUniquenessChecker checker,
            CancellationToken cancellationToken
        )
        {
            bool isUnique = await checker.IsAlbumTitleUnique(command.Title, cancellationToken);

            if (isUnique == false)
            {
                return;
            }

            if (AuthorId == command.Operator)
            {
                Title = command.Title;
            }
        }

        public void Handle(UpdateAlbumDescriptionCommand command)
        {
            if (AuthorId == command.Operator)
            {
                Description = command.Description;
            }
        }

        public void Handle(AddImageCommand command)
        {
            if (AuthorId != command.Operator || Collaborators.NotContains(command.Operator))
            {
                return;
            }

            if (Image.TryCreateNew(command, out var image))
            {
                Images.Add(image);
            }
        }

        public void Handle(DeleteImageCommand command)
        {
            if (command.Operator == AuthorId || Collaborators.Contains(command.Operator))
            {
                Images.RemoveAll(image => image.Id == command.Image);
            }
        }

        public Task Handle(
            DeleteAlbumCommand command,
            IRepository<Album, AlbumId> service,
            CancellationToken cancellationToken = default
        )
        {
            if (command.Operator == AuthorId)
            {
                return service.DeleteAsync(Id, cancellationToken);
            }

            return Task.CompletedTask;
        }

        public static async Task<Result<AlbumId>> Handle(
            CreateAlbumCommand command,
            IRepository<Album, AlbumId> repository,
            IAlbumTitleUniquenessChecker checker,
            CancellationToken cancellationToken
        )
        {
            var isUnique = await checker.IsAlbumTitleUnique(command.Title, cancellationToken);

            if (isUnique == false)
            {
                return Result.Fail<AlbumId>($"Conflict album title {command.Title}");
            }

            var album = new Album()
            {
                Title = command.Title,
                Description = command.Description,
                CategoryId = command.CategoryId,
                AuthorId = command.AuthorId,
                Accessibility = command.Accessibility
            };

            await repository.AddAsync(album, cancellationToken);

            return Result.Data(album.Id);
        }
    }
}
