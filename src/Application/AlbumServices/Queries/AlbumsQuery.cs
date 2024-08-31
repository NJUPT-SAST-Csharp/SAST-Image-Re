using Application.Query;
using Domain.Shared;

namespace Application.AlbumServices.Queries
{
    public sealed class AlbumDto(AlbumModel a)
    {
        public long Id { get; } = a.Id;
        public string Title { get; } = a.Title;
        public long Author { get; } = a.AuthorId;
        public long Category { get; } = a.CategoryId;
        public DateTime UpdatedAt { get; } = a.UpdatedAt;
    }

    public sealed record class AlbumsQuery(
        long? CategoryId,
        long? AuthorId,
        string? Title,
        Actor Actor
    ) : IQueryRequest<List<AlbumDto>> { }

    internal sealed class AlbumsQueryHandler(
        IQueryRepository<AlbumsQuery, List<AlbumDto>> repository
    ) : IQueryRequestHandler<AlbumsQuery, List<AlbumDto>>
    {
        private readonly IQueryRepository<AlbumsQuery, List<AlbumDto>> _repository = repository;

        public Task<List<AlbumDto>> Handle(AlbumsQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetOrDefaultAsync(request, cancellationToken);
        }
    }
}
