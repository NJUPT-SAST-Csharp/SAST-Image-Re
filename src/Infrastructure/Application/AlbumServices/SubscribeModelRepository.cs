using Application.AlbumServices;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Extensions;
using Domain.Shared;
using Domain.UserDomain.UserEntity;
using Infrastructure.Database;

namespace Infrastructure.Application.AlbumServices
{
    internal sealed class SubscribeModelRepository(QueryDbContext context)
        : IRepository<SubscribeModel, (AlbumId, UserId)>
    {
        private readonly QueryDbContext _context = context;

        public async Task<(AlbumId, UserId)> AddAsync(
            SubscribeModel entity,
            CancellationToken cancellationToken = default
        )
        {
            var entry = await _context.Subscribes.AddAsync(entity, cancellationToken);

            entry.Entity.Deconstruct(out long album, out long user);

            return (new(album), new(user));
        }

        public async Task DeleteAsync(
            (AlbumId, UserId) id,
            CancellationToken cancellationToken = default
        )
        {
            var subscribe = await GetOrDefaultAsync(id, cancellationToken);

            if (subscribe is not null)
                _context.Subscribes.Remove(subscribe);
        }

        public async Task<SubscribeModel> GetAsync(
            (AlbumId, UserId) id,
            CancellationToken cancellationToken = default
        )
        {
            var subscribe = await GetOrDefaultAsync(id, cancellationToken);

            if (subscribe is null)
                EntityNotFoundException.Throw(id);

            return subscribe;
        }

        public Task<SubscribeModel?> GetOrDefaultAsync(
            (AlbumId, UserId) id,
            CancellationToken cancellationToken = default
        )
        {
            return _context
                .Subscribes.FindAsync([id.Item1.Value, id.Item2.Value], cancellationToken)
                .AsTask();
        }
    }
}
