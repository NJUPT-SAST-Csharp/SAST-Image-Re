using Application;
using Application.UserServices.Queries;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UserServices.Application;

internal sealed class UserQueryRepository(QueryDbContext context)
    : IQueryRepository<UsernameExistenceQuery, UsernameExistence>
{
    public async Task<UsernameExistence> GetOrDefaultAsync(
        UsernameExistenceQuery query,
        CancellationToken cancellationToken = default
    )
    {
        bool isExist = await context.Users.AnyAsync(
            user => EF.Functions.ILike(user.Username, query.Username.Value),
            cancellationToken
        );

        return new(isExist);
    }
}
