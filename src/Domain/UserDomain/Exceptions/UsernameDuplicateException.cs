using Domain.Extensions;
using Domain.UserDomain.UserEntity;

namespace Domain.UserDomain.Exceptions
{
    public sealed class UsernameDuplicateException(Username username) : DomainException
    {
        public Username Username { get; } = username;
    }
}
