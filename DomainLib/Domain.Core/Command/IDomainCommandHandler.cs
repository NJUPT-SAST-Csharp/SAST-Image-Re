using MediatR;

namespace Domain.Command;

public interface IDomainCommandHandler<TCommand> : IRequestHandler<TCommand>
    where TCommand : IDomainCommand { }

public interface IDomainCommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : IDomainCommand<TResult> { }
