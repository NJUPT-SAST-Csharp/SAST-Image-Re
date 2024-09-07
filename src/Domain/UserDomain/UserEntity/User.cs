using Domain.Entity;
using Domain.UserDomain.Commands;
using Domain.UserDomain.Services;

namespace Domain.UserDomain.UserEntity
{
    public sealed class User : EntityBase<UserId>
    {
        private User()
            : base(default) { }

        private readonly Username _username;
        private Password _password;

        private Role[] _roles = [];
        private Roles Roles
        {
            get => new(_roles);
            set => _roles = [.. value];
        }

        public async Task<LoginResult> LoginAsync(
            LoginCommand command,
            IPasswordValidator validator,
            IJwtProvider provider,
            CancellationToken cancellationToken
        )
        {
            await validator.ValidateAsync(_password, command.Password, cancellationToken);

            string jwt = provider.GetJwt(Id, _username, Roles);

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
