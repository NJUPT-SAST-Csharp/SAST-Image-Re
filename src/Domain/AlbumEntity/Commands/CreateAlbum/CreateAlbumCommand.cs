using Domain.CategoryEntity;
using Domain.Internal.Command;
using Domain.UserEntity;

namespace Domain.AlbumEntity.Commands.CreateAlbum
{
    public readonly record struct CreateAlbumCommand(
        AlbumTitle Title,
        UserId AuthorId,
        Accessibility Accessibility,
        CategoryId CategoryId
    ) : IDomainCommand { }
}
