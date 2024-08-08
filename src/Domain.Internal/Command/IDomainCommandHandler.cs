namespace Domain.Internal.Command
{
    public interface IDomainCommandHandler<TCommand>
        where TCommand : IDomainCommand
    {
        public void Handle(ref readonly TCommand command);
    }

    public interface IDomainCommandHandler<TCommand, TResult>
        where TCommand : IDomainCommand
    {
        public TResult Handle(ref readonly TCommand command);
    }

    public interface IAsyncDomainCommandHandler<TCommand>
        where TCommand : IDomainCommand
    {
        public Task HandleAsync(
            ref readonly TCommand command,
            CancellationToken cancellationToken = default
        );
    }

    public interface IAsyncDomainCommandHandler<TCommand, TResult>
    {
        public Task<TResult> HandleAsync(
            ref readonly TCommand command,
            CancellationToken cancellationToken = default
        );
    }
}
