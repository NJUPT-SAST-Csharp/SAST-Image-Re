using Domain.Entity;
using Domain.UserDomain.Commands;
using Domain.UserDomain.Events;
using Domain.UserDomain.Services;

namespace Domain.UserDomain.UserEntity
{
    public sealed class User : EntityBase<UserId>
    {
        private User()
            : base(default) { }

        private readonly Username _username;
        private Password _password = null!;
        private Role[] _roles = [];

        private User(Username username, Password password)
            : base(UserId.GenerateNew())
        {
            _username = username;
            _password = password;
            _roles = [Role.User];
        }

        public static async Task<User> RegisterAsync(
            RegisterCommand command,
            IPasswordGenerator generator,
            CancellationToken cancellationToken
        )
        {
            var password = await generator.GenerateAsync(command.Password, cancellationToken);

            User user = new(command.Username, password);

            user.AddDomainEvent(
                new UserRegisteredEvent(user.Id, command.Username, command.Biography)
            );

            return user;
        }

        public async Task<LoginResult> LoginAsync(
            LoginCommand command,
            IPasswordValidator validator,
            IJwtProvider provider,
            CancellationToken cancellationToken
        )
        {
            await validator.ValidateAsync(_password, command.Password, cancellationToken);

            string jwt = provider.GetJwt(Id, _username, new(_roles));

            return new(jwt);
        }

        public async Task ResetPasswordAsync(
            ResetPasswordCommand command,
            IPasswordValidator validator,
            IPasswordGenerator generator,
            CancellationToken cancellationToken
        )
        {
            await validator.ValidateAsync(_password, command.OldPassword, cancellationToken);

            _password = await generator.GenerateAsync(command.NewPassword, cancellationToken);
        }
    }
}
