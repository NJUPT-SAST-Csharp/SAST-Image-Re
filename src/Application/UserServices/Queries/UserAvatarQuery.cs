using Application.Query;
using Domain.UserDomain.UserEntity;

namespace Application.UserServices.Queries;

public sealed record class UserAvatarQuery(UserId User) : IQueryRequest<Stream?> { }

internal sealed class UserAvatarQueryHandler(IAvatarStorageManager avatarStorageManager)
    : IQueryRequestHandler<UserAvatarQuery, Stream?>
{
    public Task<Stream?> Handle(UserAvatarQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(avatarStorageManager.OpenReadStream(request.User));
    }
}
