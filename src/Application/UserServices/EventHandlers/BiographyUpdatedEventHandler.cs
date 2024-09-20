using Domain.Core.Event;
using Domain.Extensions;
using Domain.UserDomain.Events;
using Domain.UserDomain.UserEntity;

namespace Application.UserServices.EventHandlers;

internal sealed class BiographyUpdatedEventHandler(IRepository<UserModel, UserId> repository)
    : IDomainEventHandler<BiographyUpdatedEvent>
{
    public async Task Handle(BiographyUpdatedEvent e, CancellationToken cancellationToken)
    {
        var user = await repository.GetAsync(e.User, cancellationToken);

        user.UpdateBiography(e);
    }
}
