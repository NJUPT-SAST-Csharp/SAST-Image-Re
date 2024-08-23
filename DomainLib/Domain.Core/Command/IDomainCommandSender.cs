namespace Domain.Command
{
    public interface IDomainCommandSender
    {
        public Task SendAsync(
            IDomainCommand command,
            CancellationToken cancellationToken = default
        );

        public Task<TResult> SendAsync<TResult>(
            IDomainCommand<TResult> command,
            CancellationToken cancellationToken = default
        );
    }
}
