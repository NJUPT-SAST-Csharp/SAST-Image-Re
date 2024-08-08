using Domain.Internal.Entity;

namespace Domain.UserEntity
{
    public readonly record struct UserId : ITypedId<UserId>
    {
        public readonly long Value { get; }

        public static UserId CreateNewId() => new();
    }
}
