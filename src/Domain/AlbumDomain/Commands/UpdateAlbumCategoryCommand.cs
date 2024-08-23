using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Services;
using Domain.CategoryDomain.CategoryEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands
{
    public readonly record struct UpdateAlbumCategoryCommand(
        AlbumId Album,
        CategoryId Category,
        Actor Actor
    ) : IDomainCommand { }

    internal sealed class UpdateAlbumCategoryCommandHandler(
        IRepository<Album, AlbumId> repository,
        ICategoryExistenceChecker checker
    ) : ICommandHandler<UpdateAlbumCategoryCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;
        private readonly ICategoryExistenceChecker _checker = checker;

        public async Task Handle(
            UpdateAlbumCategoryCommand request,
            CancellationToken cancellationToken
        )
        {
            await _checker.CheckAsync(request.Category, cancellationToken);

            var album = await _repository.GetAsync(request.Album, cancellationToken);

            album.UpdateCategory(in request);
        }
    }
}
