using Domain.Entity;

namespace Domain.UserDomain.UserEntity
{
    public sealed class User : EntityBase<UserId>
    {
        private User()
            : base(default) { }

        private readonly Username _username;

        private Role[] _roles = [];
        private Roles Roles
        {
            get => new(_roles);
            set => _roles = value.ToArray();
        }
    }
}
