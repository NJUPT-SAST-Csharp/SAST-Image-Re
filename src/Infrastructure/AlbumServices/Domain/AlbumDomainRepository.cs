using Domain.AlbumDomain.AlbumEntity;
using Domain.Extensions;
using Domain.Shared;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AlbumServices.Domain
{
    internal sealed class AlbumDomainRepository(DomainDbContext context)
        : IRepository<Album, AlbumId>
    {
        private readonly DomainDbContext _context = context;

        public async Task<AlbumId> AddAsync(
            Album entity,
            CancellationToken cancellationToken = default
        )
        {
            var entry = await _context.Albums.AddAsync(entity, cancellationToken);

            return entry.Entity.Id;
        }

        public async Task DeleteAsync(AlbumId id, CancellationToken cancellationToken = default)
        {
            var album = await GetOrDefaultAsync(id, cancellationToken);
            if (album is not null)
                _context.Albums.Remove(album);
        }

        public async Task<Album> GetAsync(AlbumId id, CancellationToken cancellationToken = default)
        {
            var album = await _context.Albums.FirstOrDefaultAsync(
                a => a.Id == id,
                cancellationToken
            );

            if (album is null)
            {
                EntityNotFoundException.Throw(id);
            }

            return album;
        }

        public Task<Album?> GetOrDefaultAsync(
            AlbumId id,
            CancellationToken cancellationToken = default
        )
        {
            return _context.Albums.FirstOrDefaultAsync(album => album.Id == id, cancellationToken);
        }
    }
}
