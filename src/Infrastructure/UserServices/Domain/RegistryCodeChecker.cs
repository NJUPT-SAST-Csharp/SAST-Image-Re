using Domain.UserDomain.Exceptions;
using Domain.UserDomain.Services;
using Domain.UserDomain.UserEntity;
using Infrastructure.SharedServices.Cache;
using StackExchange.Redis;

namespace Infrastructure.UserServices.Domain;

internal sealed class RegistryCodeChecker(IConnectionMultiplexer connection) : IRegistryCodeChecker
{
    private readonly IDatabase _database = connection.GetDatabase();

    public async Task CheckAsync(
        Username username,
        RegistryCode code,
        CancellationToken cancellationToken
    )
    {
        var cacheCode = await _database
            .HashGetAsync(CacheKeys.RegistryCodes, username.Value)
            .WaitAsync(cancellationToken);

        if (cacheCode.IsNullOrEmpty || cacheCode.TryParse(out int actualCode) == false)
            throw new RegistryCodeException();

        if (code.Value != actualCode)
            throw new RegistryCodeException();

        await _database.HashDeleteAsync(CacheKeys.RegistryCodes, username.Value);
    }
}
