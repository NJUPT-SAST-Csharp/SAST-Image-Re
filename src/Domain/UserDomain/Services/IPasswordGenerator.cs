using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Services
{
    public interface IPasswordGenerator
    {
        public Task<Password> GenerateAsync(
            PasswordInput password,
            CancellationToken cancellationToken
        );
    }
}
