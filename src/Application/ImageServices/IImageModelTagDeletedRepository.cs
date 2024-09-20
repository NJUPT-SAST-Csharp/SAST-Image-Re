using Domain.TagDomain.TagEntity;

namespace Application.ImageServices;

public interface IImageModelTagDeletedRepository
{
    public Task<List<ImageModel>> GetAsync(TagId id, CancellationToken cancellationToken);
}
