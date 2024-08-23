using System.Diagnostics.CodeAnalysis;
using Domain.Extensions;

namespace Domain.AlbumDomain.Exceptions
{
    public sealed class NullCoverImageWhenNotLatestImageException : DomainException
    {
        [DoesNotReturn]
        public static void Throw() => throw new NullCoverImageWhenNotLatestImageException();
    }
}
