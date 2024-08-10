using Domain.Command;
using Domain.UserEntity;

namespace Domain.AlbumEntity.Commands
{
    public readonly record struct UpdateAlbumDescriptionCommand(
        AlbumId Album,
        AlbumDescription Description,
        UserId Operator
    ) : IDomainCommand { }
}
