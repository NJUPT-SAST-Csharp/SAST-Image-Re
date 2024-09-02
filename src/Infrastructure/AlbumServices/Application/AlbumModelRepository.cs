using Application.AlbumServices;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Extensions;
using Domain.Shared;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AlbumServices.Application
{
    internal sealed class AlbumModelRepository(QueryDbContext context)
        : IRepository<AlbumModel, AlbumId>
    {
        private readonly QueryDbContext _context = context;

        public async Task<AlbumModel> GetAsync(
            AlbumId id,
            CancellationToken cancellationToken = default
        )
        {
            var album = await GetOrDefaultAsync(id, cancellationToken);

            if (album is null)
                EntityNotFoundException.Throw(id);

            return album;
        }

        public Task<AlbumModel?> GetOrDefaultAsync(
            AlbumId id,
            CancellationToken cancellationToken = default
        )
        {
            return _context
                .Albums.Include(album => album.Images)
                .Include(album => album.Subscribes)
                .FirstOrDefaultAsync(a => a.Id == id.Value, cancellationToken);
        }

        public async Task<AlbumId> AddAsync(
            AlbumModel entity,
            CancellationToken cancellationToken = default
        )
        {
            var album = await _context.Albums.AddAsync(entity, cancellationToken);
            return new(album.Entity.Id);
        }

        public async Task DeleteAsync(AlbumId id, CancellationToken cancellationToken = default)
        {
            var album = await GetOrDefaultAsync(id, cancellationToken);

            if (album is not null)
                _context.Albums.Remove(album);
        }
    }
}
