using Domain.Extensions;
using Domain.Shared;
using Domain.UserDomain.UserEntity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UserServices.Domain
{
    internal sealed class UserDomainRepository(DomainDbContext context)
        : IRepository<User, Username>,
            IRepository<User, UserId>
    {
        private readonly DomainDbContext _context = context;

        Task<Username> IRepository<User, Username>.AddAsync(
            User entity,
            CancellationToken cancellationToken
        )
        {
            throw new InvalidOperationException(
                "UserRepository using [Username] couldn't be used to complete the operation."
            );
        }

        async Task<UserId> IRepository<User, UserId>.AddAsync(
            User entity,
            CancellationToken cancellationToken
        )
        {
            var entry = await _context.Users.AddAsync(entity, cancellationToken);

            return entry.Entity.Id;
        }

        Task IRepository<User, Username>.DeleteAsync(
            Username id,
            CancellationToken cancellationToken
        )
        {
            throw new InvalidOperationException(
                "UserRepository using [Username] couldn't be used to complete the operation."
            );
        }

        async Task IRepository<User, UserId>.DeleteAsync(
            UserId id,
            CancellationToken cancellationToken
        )
        {
            var user =
                await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                ?? throw new EntityNotFoundException();

            _context.Users.Remove(user);
        }

        async Task<User> IRepository<User, Username>.GetAsync(
            Username username,
            CancellationToken cancellationToken
        )
        {
            return await _context
                    .Users.FromSql(
                        $"SELECT * FROM domain.users WHERE username ILIKE {username.Value}"
                    )
                    .FirstOrDefaultAsync(cancellationToken) ?? throw new EntityNotFoundException();
        }

        async Task<User> IRepository<User, UserId>.GetAsync(
            UserId id,
            CancellationToken cancellationToken
        )
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                ?? throw new EntityNotFoundException();
        }

        Task<User?> IRepository<User, Username>.GetOrDefaultAsync(
            Username username,
            CancellationToken cancellationToken
        )
        {
            return _context
                .Users.FromSql($"SELECT * FROM domain.users WHERE username ILIKE {username.Value}")
                .FirstOrDefaultAsync(cancellationToken);
        }

        Task<User?> IRepository<User, UserId>.GetOrDefaultAsync(
            UserId id,
            CancellationToken cancellationToken
        )
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
