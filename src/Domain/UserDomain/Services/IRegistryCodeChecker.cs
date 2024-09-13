using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Services
{
    public interface IRegistryCodeChecker
    {
        public Task CheckAsync(
            Username username,
            RegistryCode code,
            CancellationToken cancellationToken
        );
    }
}
