using MediatR;

namespace Application.Query
{
    public interface IQueryRequest<TResult> : IRequest<TResult> { }
}
