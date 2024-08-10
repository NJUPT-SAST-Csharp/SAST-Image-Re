using Domain.Command;
using Domain.ImageEntity;
using Domain.UserEntity;

namespace Domain.AlbumEntity.Commands
{
    public readonly record struct DeleteImageCommand(AlbumId Album, ImageId Image, UserId Operator)
        : IDomainCommand { }
}
