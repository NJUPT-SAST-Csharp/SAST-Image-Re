using Domain.Command;
using Domain.UserEntity;

namespace Domain.AlbumEntity.Commands
{
    public readonly record struct DeleteAlbumCommand(AlbumId Album, UserId Operator)
        : IDomainCommand { }
}
