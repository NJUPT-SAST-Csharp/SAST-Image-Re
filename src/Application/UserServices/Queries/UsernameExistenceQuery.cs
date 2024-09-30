using Application.Query;
using Domain.UserDomain.UserEntity;

namespace Application.UserServices.Queries;

public readonly record struct UsernameExistence(bool IsExist);

public sealed record class UsernameExistenceQuery(Username Username)
    : IQueryRequest<UsernameExistence>;

internal sealed class UsernameExistenceQueryHandler(
    IQueryRepository<UsernameExistenceQuery, UsernameExistence> repository
) : IQueryRequestHandler<UsernameExistenceQuery, UsernameExistence>
{
    public Task<UsernameExistence> Handle(
        UsernameExistenceQuery request,
        CancellationToken cancellationToken
    )
    {
        return repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
