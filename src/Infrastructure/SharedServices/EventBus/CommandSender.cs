using Domain.Command;
using MediatR;

namespace Infrastructure.SharedServices.EventBus;

internal sealed class CommandSender(IMediator mediator) : IDomainCommandSender
{
    private readonly IMediator _mediator = mediator;

    public Task SendAsync(IDomainCommand command, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(command, cancellationToken);
    }

    public Task<TResult> SendAsync<TResult>(
        IDomainCommand<TResult> command,
        CancellationToken cancellationToken = default
    )
    {
        return _mediator.Send(command, cancellationToken);
    }
}
