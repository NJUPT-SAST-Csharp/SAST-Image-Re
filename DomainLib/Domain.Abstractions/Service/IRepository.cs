using Domain.Entity;

namespace Domain.Service
{
    public interface IRepository<TEntity, TId> : IDomainService
        where TEntity : IEntity<TId>
        where TId : IEquatable<TId>
    {
        public Task<TEntity> GetAsync(TId id, CancellationToken cancellationToken = default);

        public Task<TEntity?> GetOrDefaultAsync(
            TId id,
            CancellationToken cancellationToken = default
        );

        public Task DeleteAsync(TId id, CancellationToken cancellationToken = default);

        public Task<TId> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
