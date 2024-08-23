using System.Diagnostics.CodeAnalysis;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Extensions;

namespace Domain.AlbumDomain.Exceptions
{
    public sealed class AlbumTitleDuplicateException(AlbumTitle title) : DomainException
    {
        public AlbumTitle Title { get; } = title;

        [DoesNotReturn]
        public static void Throw(AlbumTitle title) => throw new AlbumTitleDuplicateException(title);
    }
}
