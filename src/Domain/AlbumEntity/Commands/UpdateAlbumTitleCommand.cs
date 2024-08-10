using Domain.Command;
using Domain.UserEntity;

namespace Domain.AlbumEntity.Commands
{
    public readonly record struct UpdateAlbumTitleCommand(
        AlbumId Album,
        AlbumTitle Title,
        UserId Operator
    ) : IDomainCommand { }
}
