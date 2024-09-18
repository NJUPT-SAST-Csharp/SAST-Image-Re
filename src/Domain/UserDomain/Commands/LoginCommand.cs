using Domain.Command;
using Domain.Extensions;
using Domain.UserDomain.Exceptions;
using Domain.UserDomain.Services;
using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Commands;

public sealed record LoginCommand(Username Username, PasswordInput Password)
    : IDomainCommand<JwtValue>;

internal sealed class LoginCommandHandler(
    IRepository<User, Username> repository,
    IPasswordValidator validator,
    IJwtGenerator jwtGenerator
) : IDomainCommandHandler<LoginCommand, JwtValue>
{
    public async Task<JwtValue> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user =
            await repository.GetOrDefaultAsync(command.Username, cancellationToken)
            ?? throw new LoginException();

        return await user.LoginAsync(command, validator, jwtGenerator, cancellationToken);
    }
}
