using Domain.TagDomain.TagEntity;

namespace Domain.TagDomain.Services;

public interface ITagNameUniquenessChecker
{
    public Task CheckAsync(TagName name, CancellationToken cancellationToken = default);
}
