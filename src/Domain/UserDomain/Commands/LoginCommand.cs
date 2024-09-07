using Domain.Command;
using Domain.Extensions;
using Domain.UserDomain.Exceptions;
using Domain.UserDomain.Services;
using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Commands
{
    public record LoginResult(string Jwt);

    public sealed record LoginCommand(Username Username, PasswordInput Password)
        : IDomainCommand<LoginResult>;

    internal sealed class LoginCommandHandler(
        IRepository<User, Username> repository,
        IPasswordValidator validator,
        IJwtProvider provider
    ) : IDomainCommandHandler<LoginCommand, LoginResult>
    {
        private readonly IRepository<User, Username> _repository = repository;
        private readonly IPasswordValidator _validator = validator;
        private readonly IJwtProvider _provider = provider;

        public async Task<LoginResult> Handle(
            LoginCommand command,
            CancellationToken cancellationToken
        )
        {
            var user =
                await _repository.GetOrDefaultAsync(command.Username, cancellationToken)
                ?? throw new LoginException();

            return await user.LoginAsync(command, _validator, _provider, cancellationToken);
        }
    }
}
