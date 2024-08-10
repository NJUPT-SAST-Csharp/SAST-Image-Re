using MediatR;

namespace Domain.Command
{
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand>
        where TCommand : IDomainCommand { }

    public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
        where TCommand : IDomainCommand<TResult> { }
}
