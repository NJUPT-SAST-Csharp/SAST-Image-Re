using Domain.Command;
using Domain.Extensions;
using Domain.UserDomain.Services;
using Domain.UserDomain.UserEntity;
using Domain.UserEntity.Services;

namespace Domain.UserDomain.Commands
{
    public sealed record RegisterCommand(
        Username Username,
        PasswordInput Password,
        RegistryCode Code
    ) : IDomainCommand<JwtValue>;

    internal sealed class RegisterCommandHandler(
        IRegistryCodeChecker codeChecker,
        IUsernameUniquenessChecker usernameChecker,
        IPasswordGenerator pwdGenerator,
        IJwtGenerator jwtGenerator,
        IRepository<User, UserId> repository
    ) : IDomainCommandHandler<RegisterCommand, JwtValue>
    {
        public async Task<JwtValue> Handle(
            RegisterCommand command,
            CancellationToken cancellationToken
        )
        {
            await Task.WhenAll(
                usernameChecker.CheckAsync(command.Username, cancellationToken),
                codeChecker.CheckAsync(command.Username, command.Code, cancellationToken)
            );

            var result = await User.RegisterAsync(
                command,
                pwdGenerator,
                jwtGenerator,
                cancellationToken
            );

            await repository.AddAsync(result.User, cancellationToken);

            return result.Jwt;
        }
    }
}
