using Application.Query;
using Domain.AlbumDomain.ImageEntity;
using Domain.Shared;

namespace Application.ImageServices.Queries;

public sealed class DetailedImage(ImageModel i, int likeCount, bool isLiked)
{
    public long Id { get; } = i.Id;
    public long AlbumId { get; } = i.AlbumId;
    public long UploaderId { get; } = i.UploaderId;
    public string Title { get; } = i.Title;
    public DateTime UploadedAt { get; } = i.UploadedAt;
    public long[] Tags { get; } = i.Tags;
    public int LikeCount { get; } = likeCount;
    public RequesterInfo Requester { get; } = new(isLiked);

    public readonly record struct RequesterInfo(bool Liked);
}

public sealed record DetailedImageQuery(ImageId Image, Actor Actor) : IQueryRequest<DetailedImage?>;

internal sealed class DetailedImageQueryHandler(
    IQueryRepository<DetailedImageQuery, DetailedImage?> repository
) : IQueryRequestHandler<DetailedImageQuery, DetailedImage?>
{
    private readonly IQueryRepository<DetailedImageQuery, DetailedImage?> _repository = repository;

    public Task<DetailedImage?> Handle(
        DetailedImageQuery request,
        CancellationToken cancellationToken
    )
    {
        return _repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
