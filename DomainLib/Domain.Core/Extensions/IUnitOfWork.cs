namespace Domain.Extensions
{
    public interface IUnitOfWork : IDomainService
    {
        public Task CommitChangesAsync(CancellationToken cancellationToken = default);
    }
}
