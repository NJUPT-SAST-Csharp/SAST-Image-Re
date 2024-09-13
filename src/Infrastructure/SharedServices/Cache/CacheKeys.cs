using StackExchange.Redis;

namespace Infrastructure.SharedServices.Cache
{
    internal readonly record struct CacheKeys(string Value)
    {
        public static CacheKeys RegistryCodes { get; } = new(nameof(RegistryCodes));

        public static implicit operator RedisKey(CacheKeys cacheKey)
        {
            return cacheKey.Value;
        }
    }
}
