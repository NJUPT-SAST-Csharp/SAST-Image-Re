using Application.Query;
using Domain.Shared;

namespace Application.AlbumServices.Queries
{
    public sealed record class AlbumDto(
        long Id,
        string Title,
        long Author,
        long Category,
        bool IsArchived,
        DateTime UpdatedAt
    )
    {
        public AlbumDto(AlbumModel album)
            : this(
                album.Id,
                album.Title,
                album.AuthorId,
                album.CategoryId,
                album.IsArchived,
                album.UpdatedAt
            ) { }
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
