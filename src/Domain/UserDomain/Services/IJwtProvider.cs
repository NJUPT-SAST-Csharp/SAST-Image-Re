using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Services
{
    public interface IJwtProvider
    {
        public string GetJwt(UserId id, Username username, Roles roles);
    }
}
