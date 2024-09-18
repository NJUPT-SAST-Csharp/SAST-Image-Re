using Application.UserServices;
using Domain.Extensions;
using Domain.Shared;
using Domain.UserDomain.UserEntity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UserServices.Application;

internal sealed class UserModelRepository(QueryDbContext context) : IRepository<UserModel, UserId>
{
    public async Task<UserId> AddAsync(
        UserModel entity,
        CancellationToken cancellationToken = default
    )
    {
        var entry = await context.Users.AddAsync(entity, cancellationToken);
        long id = entry.Entity.Id;
        return new(id);
    }

    public async Task DeleteAsync(UserId id, CancellationToken cancellationToken = default)
    {
        var user = await GetAsync(id, cancellationToken);
        context.Users.Remove(user);
    }

    public async Task<UserModel> GetAsync(UserId id, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(
                user => user.Id == id.Value,
                cancellationToken
            ) ?? throw new EntityNotFoundException();
    }

    public Task<UserModel?> GetOrDefaultAsync(
        UserId id,
        CancellationToken cancellationToken = default
    )
    {
        return context.Users.FirstOrDefaultAsync(user => user.Id == id.Value, cancellationToken);
    }
}
