using Domain.UserDomain.Events;

namespace Application.UserServices
{
    public sealed class UserModel
    {
        private UserModel() { }

        public long Id { get; }
        public string Username { get; private set; } = null!;
        public string Biography { get; private set; } = string.Empty;
        public Uri? AvatarUrl { get; private set; } = null;
        public Uri? HeaderUrl { get; private set; } = null;

        internal UserModel(UserRegisteredEvent e)
        {
            Id = e.Id.Value;
            Username = e.Username.Value;
            Biography = e.Biography.Value;
        }

        internal void ResetUsername(UsernameResetEvent e)
        {
            Username = e.Username.Value;
        }
    }
}
