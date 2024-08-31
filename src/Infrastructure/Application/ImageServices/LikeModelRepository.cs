using Application.ImageServices;
using Domain.AlbumDomain.ImageEntity;
using Domain.Extensions;
using Domain.Shared;
using Domain.UserDomain.UserEntity;
using Infrastructure.Database;

namespace Infrastructure.Application.ImageServices
{
    internal sealed class LikeModelRepository(QueryDbContext context)
        : IRepository<LikeModel, (ImageId, UserId)>
    {
        private readonly QueryDbContext _context = context;

        public async Task<(ImageId, UserId)> AddAsync(
            LikeModel entity,
            CancellationToken cancellationToken = default
        )
        {
            var entry = await _context.Likes.AddAsync(entity, cancellationToken);
            entry.Entity.Deconstruct(out long image, out long user);

            return (new(image), new(user));
        }

        public async Task DeleteAsync(
            (ImageId, UserId) id,
            CancellationToken cancellationToken = default
        )
        {
            var like = await GetOrDefaultAsync(id, cancellationToken);

            if (like is not null)
                _context.Likes.Remove(like);
        }

        public async Task<LikeModel> GetAsync(
            (ImageId, UserId) id,
            CancellationToken cancellationToken = default
        )
        {
            var like = await GetOrDefaultAsync(id, cancellationToken);

            if (like is null)
                EntityNotFoundException.Throw(id);

            return like;
        }

        public Task<LikeModel?> GetOrDefaultAsync(
            (ImageId, UserId) id,
            CancellationToken cancellationToken = default
        )
        {
            return _context
                .Likes.FindAsync([id.Item1.Value, id.Item2.Value], cancellationToken)
                .AsTask();
        }
    }
}
