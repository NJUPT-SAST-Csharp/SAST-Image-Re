using Domain.Command;
using Domain.Extensions;
using Domain.Shared;
using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Commands
{
    public sealed record UpdateBiographyCommand(Biography Biography, Actor Actor)
        : IDomainCommand { }

    internal sealed class UpdateBiographyCommandHandler(IRepository<User, UserId> repository)
        : IDomainCommandHandler<UpdateBiographyCommand>
    {
        public async Task Handle(UpdateBiographyCommand e, CancellationToken cancellationToken)
        {
            var user = await repository.GetAsync(e.Actor.Id, cancellationToken);

            user.UpdateBiography(e);
        }
    }
}
