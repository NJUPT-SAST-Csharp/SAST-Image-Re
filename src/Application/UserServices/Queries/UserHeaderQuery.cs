using Application.Query;
using Domain.UserDomain.UserEntity;

namespace Application.UserServices.Queries;

public sealed record UserHeaderQuery(UserId User) : IQueryRequest<Stream?> { }

internal sealed class UserHeaderQueryHandler(IHeaderStorageManager manager)
    : IQueryRequestHandler<UserHeaderQuery, Stream?>
{
    public Task<Stream?> Handle(UserHeaderQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(manager.OpenReadStream(request.User));
    }
}
