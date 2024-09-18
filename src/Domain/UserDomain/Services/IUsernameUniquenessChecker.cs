using Domain.UserDomain.UserEntity;

namespace Domain.UserEntity.Services;

public interface IUsernameUniquenessChecker
{
    public Task CheckAsync(Username username, CancellationToken cancellationToken = default);
}
