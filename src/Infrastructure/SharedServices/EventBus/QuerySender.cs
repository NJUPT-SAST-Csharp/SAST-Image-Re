using Application.Query;
using MediatR;

namespace Infrastructure.SharedServices.EventBus;

internal sealed class QuerySender(IMediator mediator) : IQueryRequestSender
{
    private readonly IMediator _mediator = mediator;

    public Task<TResult> SendAsync<TResult>(
        IQueryRequest<TResult> request,
        CancellationToken cancellationToken = default
    )
    {
        return _mediator.Send(request, cancellationToken);
    }
}
