using Application.Query;
using Domain.AlbumDomain.ImageEntity;
using Domain.Shared;

namespace Application.ImageServices.Queries;

public sealed record ImageFileQuery(ImageId Image, ImageKind Kind, Actor Actor)
    : IQueryRequest<Stream?>;

internal sealed class ImageFileQueryHandler(
    IImageStorageManager manager,
    IImageAvailabilityChecker checker
) : IQueryRequestHandler<ImageFileQuery, Stream?>
{
    private readonly IImageStorageManager _manager = manager;
    private readonly IImageAvailabilityChecker _checker = checker;

    public async Task<Stream?> Handle(ImageFileQuery request, CancellationToken cancellationToken)
    {
        bool result = await _checker.CheckAsync(request.Image, request.Actor, cancellationToken);

        if (result == false)
            return null;

        return _manager.OpenReadStream(request.Image, request.Kind);
    }
}
