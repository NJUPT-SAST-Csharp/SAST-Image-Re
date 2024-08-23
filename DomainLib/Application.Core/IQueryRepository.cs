using Application.Query;

namespace Application
{
    public interface IQueryRepository<TQuery, TQueryResponse>
        where TQuery : IQueryRequest<TQueryResponse>
    {
        public Task<TQueryResponse> GetOrDefaultAsync(
            TQuery query,
            CancellationToken cancellationToken = default
        );
    }
}
