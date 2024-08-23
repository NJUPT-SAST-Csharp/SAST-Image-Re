using Domain.Entity;

namespace Domain.UserDomain.UserEntity
{
    public readonly record struct UserId(long Value) : ITypedId<UserId, long>
    {
        public static UserId GenerateNew() => new(Snowflake.NewId);
    }
}
