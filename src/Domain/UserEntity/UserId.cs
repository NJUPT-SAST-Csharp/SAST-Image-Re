using Domain.Internal;

namespace Domain.UserEntity
{
    internal readonly record struct UserId : ITypedId<UserId>
    {
        public readonly long Value { get; }

        public static UserId CreateNewId() => new();
    }
}
