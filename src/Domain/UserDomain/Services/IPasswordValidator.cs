using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Services
{
    public interface IPasswordValidator
    {
        public Task ValidateAsync(
            Password password,
            PasswordInput input,
            CancellationToken cancellationToken
        );
    }
}
