using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Exceptions;
using Domain.AlbumDomain.Services;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Domain.AlbumServices
{
    internal sealed class CollaboratorsExistenceChecker(DomainDbContext context)
        : ICollaboratorsExistenceChecker
    {
        private readonly DomainDbContext _context = context;

        public async Task CheckAsync(
            Collaborators collaborators,
            CancellationToken cancellationToken = default
        )
        {
            bool isAllExisting = await _context
                .Users.AsNoTracking()
                .AllAsync(user => collaborators.Contains(user.Id), cancellationToken);

            if (isAllExisting == false)
                CollaboratorsNotFoundException.Throw(collaborators);
        }
    }
}
