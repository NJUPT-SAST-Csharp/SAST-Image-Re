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

        private Username _username;
        private Password _password = null!;
        private Role[] _roles = [];

        private User(Username username, Password password)
            : base(UserId.GenerateNew())
        {
            _username = username;
            _password = password;
            _roles = [Role.User];
        }

        public static async Task<(User User, JwtValue Jwt)> RegisterAsync(
            RegisterCommand command,
            IPasswordGenerator pwdGenerator,
            IJwtGenerator jwtGenerator,
            CancellationToken cancellationToken
        )
        {
            var password = await pwdGenerator.GenerateAsync(command.Password, cancellationToken);

            User user = new(command.Username, password);

            user.AddDomainEvent(new UserRegisteredEvent(user.Id, command.Username));

            var jwt = jwtGenerator.GetJwt(user.Id, user._username, new(user._roles));

            return (user, jwt);
        }

        public async Task<JwtValue> LoginAsync(
            LoginCommand command,
            IPasswordValidator validator,
            IJwtGenerator generator,
            CancellationToken cancellationToken
        )
        {
            await validator.ValidateAsync(_password, command.Password, cancellationToken);

            var jwt = generator.GetJwt(Id, _username, new(_roles));

            return jwt;
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

        public void ResetUsername(ResetUsernameCommand command)
        {
            _username = command.Username;

            AddDomainEvent(new UsernameResetEvent(Id, _username));
        }

        public void UpdateBiography(UpdateBiographyCommand command)
        {
            AddDomainEvent(new BiographyUpdatedEvent(Id, command.Biography));
        }
    }
}
