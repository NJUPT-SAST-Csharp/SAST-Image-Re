using Domain.UserDomain.UserEntity;

namespace Domain.Shared
{
    public readonly record struct Actor
    {
        public readonly UserId Id { get; init; }
        public readonly bool IsAuthenticated { get; init; }
        public readonly bool IsAdmin { get; init; }
    }
}
