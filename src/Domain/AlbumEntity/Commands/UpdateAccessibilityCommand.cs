using Domain.Internal.Command;
using Domain.UserEntity;

namespace Domain.AlbumEntity.Commands
{
    public readonly record struct UpdateAccessibilityCommand(
        AlbumId Album,
        Accessibility Accessibility,
        UserId Operator
    ) : IDomainCommand { }
}
