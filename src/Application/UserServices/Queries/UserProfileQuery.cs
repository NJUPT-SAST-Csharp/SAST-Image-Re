using Application.Query;
using Domain.UserDomain.UserEntity;

namespace Application.UserServices.Queries;

public sealed record class UserProfileDto(long Id, string Username, string Biography);

public sealed record UserProfileQuery(UserId User) : IQueryRequest<UserProfileDto?> { }

internal sealed class UserProfileQueryHandler(
    IQueryRepository<UserProfileQuery, UserProfileDto?> repository
) : IQueryRequestHandler<UserProfileQuery, UserProfileDto?>
{
    public Task<UserProfileDto?> Handle(
        UserProfileQuery request,
        CancellationToken cancellationToken
    )
    {
        return repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
