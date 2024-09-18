using System.Diagnostics.CodeAnalysis;
using Domain.AlbumDomain.ImageEntity;
using Domain.Extensions;

namespace Domain.AlbumDomain.Exceptions;

public sealed class ImageNotFoundException(ImageId imageId) : DomainException
{
    public ImageId ImageId { get; } = imageId;

    [DoesNotReturn]
    public static void Throw(ImageId imageId) => throw new ImageNotFoundException(imageId);
}
