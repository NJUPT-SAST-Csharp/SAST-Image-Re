using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Exceptions;
using Domain.AlbumDomain.Services;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AlbumServices.Domain
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
            var userIdsInDb = await _context
                .Users.AsNoTracking()
                .Select(u => u.Id)
                .Where(u => collaborators.Contains(u))
                .ToListAsync(cancellationToken);

            bool isAllExisting = collaborators.All(userIdsInDb.Contains);

            if (isAllExisting == false)
                CollaboratorsNotFoundException.Throw(collaborators);
        }
    }
}
