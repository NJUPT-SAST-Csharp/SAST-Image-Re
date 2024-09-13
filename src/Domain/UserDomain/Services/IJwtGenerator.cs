using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Services
{
    public interface IJwtGenerator
    {
        public JwtValue GetJwt(UserId id, Username username, Roles roles);
    }
}
