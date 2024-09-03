using Application.Query;

namespace Application.TagServices.Queries
{
    public sealed record TagDto(long Id, string Name) { }

    public sealed record TagsQuery(string? Name) : IQueryRequest<List<TagDto>> { }

    internal sealed class TagsQueryHandler(IQueryRepository<TagsQuery, List<TagDto>> repository)
        : IQueryRequestHandler<TagsQuery, List<TagDto>>
    {
        private readonly IQueryRepository<TagsQuery, List<TagDto>> _repository = repository;

        public Task<List<TagDto>> Handle(TagsQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetOrDefaultAsync(request, cancellationToken);
        }
    }
}
