using Domain.Extensions;
using Domain.TagDomain.TagEntity;

namespace Domain.TagDomain
{
    public sealed class TagNameDuplicateException(TagName name) : DomainException
    {
        public TagName Name { get; } = name;
    }
}
