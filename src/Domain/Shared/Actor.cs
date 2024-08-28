using System.Security.Claims;
using Domain.UserDomain.UserEntity;

namespace Domain.Shared
{
    public readonly record struct Actor
    {
        public readonly UserId Id { get; init; }
        public readonly bool IsAuthenticated { get; init; }
        public readonly bool IsAdmin { get; init; }

        public Actor(ClaimsPrincipal user)
        {
            Id = new(1);

            // TODO: Implement the logic to get the user id from the claims
        }
    }
}
