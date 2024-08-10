namespace Domain.Command
{
    public interface ICommandSender
    {
        public Task SendAsync<TCommand>(
            TCommand command,
            CancellationToken cancellationToken = default
        )
            where TCommand : IDomainCommand;

        public Task<TResult> SendAsync<TCommand, TResult>(
            TCommand command,
            CancellationToken cancellationToken = default
        )
            where TCommand : IDomainCommand<TResult>;
    }
}
