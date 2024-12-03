using Application.Query;
using Domain.Shared;
using Domain.UserDomain.UserEntity;

namespace Application.ImageServices.Queries;

public sealed record class UserImagesQuery(UserId User, Actor Actor)
    : IQueryRequest<List<ImageDto>> { }

internal sealed class UserImagesQueryHandler(
    IQueryRepository<UserImagesQuery, List<ImageDto>> repository
) : IQueryRequestHandler<UserImagesQuery, List<ImageDto>>
{
    public Task<List<ImageDto>> Handle(UserImagesQuery request, CancellationToken cancellationToken)
    {
        return repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
