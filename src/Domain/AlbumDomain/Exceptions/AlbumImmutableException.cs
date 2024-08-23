using System.Diagnostics.CodeAnalysis;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Extensions;

namespace Domain.AlbumDomain.Exceptions
{
    public sealed class AlbumImmutableException(Album album) : DomainException
    {
        public AlbumId Id { get; } = album.Id;

        [DoesNotReturn]
        public static void Throw(Album album) => throw new AlbumImmutableException(album);
    }
}
