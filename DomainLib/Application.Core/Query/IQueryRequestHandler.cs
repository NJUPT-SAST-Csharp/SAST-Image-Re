using MediatR;

namespace Application.Query;

public interface IQueryRequestHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQueryRequest<TResult> { }
