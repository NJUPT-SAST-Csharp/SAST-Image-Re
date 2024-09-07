using Domain.Extensions;
using Domain.Shared;
using Domain.UserDomain.UserEntity;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UserServices.Domain
{
    internal sealed class UserDomainRepository(DomainDbContext context)
        : IRepository<User, Username>
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

        Task IRepository<User, Username>.DeleteAsync(
            Username id,
            CancellationToken cancellationToken
        )
        {
            throw new InvalidOperationException(
                "UserRepository using [Username] couldn't be used to complete the operation."
            );
        }

        async Task<User> IRepository<User, Username>.GetAsync(
            Username username,
            CancellationToken cancellationToken
        )
        {
            return await _context
                    .Users.Where(user => EF.Property<Username>(user, "_username") == username)
                    .FirstOrDefaultAsync(cancellationToken) ?? throw new EntityNotFoundException();
        }

        Task<User?> IRepository<User, Username>.GetOrDefaultAsync(
            Username username,
            CancellationToken cancellationToken
        )
        {
            return _context
                .Users.Where(user => EF.Property<Username>(user, "_username") == username)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
