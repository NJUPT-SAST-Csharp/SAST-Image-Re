namespace Application.Query;

public interface IQueryRequestSender
{
    public Task<TResult> SendAsync<TResult>(
        IQueryRequest<TResult> request,
        CancellationToken cancellationToken = default
    );
}
