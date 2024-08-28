namespace Domain.Extensions
{
    public interface IRepository<TEntity, TId> : IDomainService
        where TEntity : class
        where TId : IEquatable<TId>
    {
        public Task<TEntity> GetAsync(TId id, CancellationToken cancellationToken = default);

        public Task<TEntity?> GetOrDefaultAsync(
            TId id,
            CancellationToken cancellationToken = default
        );
        public Task<TId> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    }

    public interface IRepository<TEntity> : IDomainService
        where TEntity : class
    {
        public Task<List<TEntity>> GetAsync(CancellationToken cancellationToken);
    }
}
