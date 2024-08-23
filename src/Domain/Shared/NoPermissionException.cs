using System.Diagnostics.CodeAnalysis;
using Domain.Extensions;

namespace Domain.Shared
{
    public sealed class NoPermissionException() : DomainException
    {
        [DoesNotReturn]
        public static void Throw() => throw new NoPermissionException();
    }
}
