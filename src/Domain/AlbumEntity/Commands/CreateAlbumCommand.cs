using Domain.CategoryEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.UserEntity;

namespace Domain.AlbumEntity.Commands
{
    public readonly record struct CreateAlbumCommand(
        AlbumTitle Title,
        AlbumDescription Description,
        UserId AuthorId,
        Accessibility Accessibility,
        CategoryId CategoryId
    ) : IDomainCommand<Result<AlbumId>>
    { }
}
