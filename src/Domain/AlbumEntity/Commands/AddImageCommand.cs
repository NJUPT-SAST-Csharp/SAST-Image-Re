using Domain.Command;
using Domain.ImageEntity;
using Domain.UserEntity;

namespace Domain.AlbumEntity.Commands
{
    public readonly record struct AddImageCommand(AlbumId Album, ImageTitle Title, UserId Operator)
        : IDomainCommand { }
}
