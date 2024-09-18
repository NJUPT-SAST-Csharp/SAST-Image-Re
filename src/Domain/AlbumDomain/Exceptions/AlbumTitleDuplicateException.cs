using Domain.AlbumDomain.AlbumEntity;
using Domain.Extensions;

namespace Domain.AlbumDomain.Exceptions;

public sealed class AlbumTitleDuplicateException(AlbumTitle title) : DomainException
{
    public AlbumTitle Title { get; } = title;
}
