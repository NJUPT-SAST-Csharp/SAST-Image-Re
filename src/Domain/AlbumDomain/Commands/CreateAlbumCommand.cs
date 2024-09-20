using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Services;
using Domain.CategoryDomain.CategoryEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands;

public sealed record class CreateAlbumCommand(
    AlbumTitle Title,
    AlbumDescription Description,
    AccessLevel AccessLevel,
    CategoryId CategoryId,
    Actor Actor
) : IDomainCommand<AlbumId> { }

internal sealed class CreateAlbumCommandHandler(
    IRepository<Album, AlbumId> repository,
    IAlbumTitleUniquenessChecker titleChecker,
    ICategoryExistenceChecker categoryChecker
) : IDomainCommandHandler<CreateAlbumCommand, AlbumId>
{
    private readonly IRepository<Album, AlbumId> _repository = repository;
    private readonly IAlbumTitleUniquenessChecker _titleChecker = titleChecker;
    private readonly ICategoryExistenceChecker _categoryChecker = categoryChecker;

    public async Task<AlbumId> Handle(
        CreateAlbumCommand command,
        CancellationToken cancellationToken
    )
    {
        await _titleChecker.CheckAsync(command.Title, cancellationToken);
        await _categoryChecker.CheckAsync(command.CategoryId, cancellationToken);

        Album album = new(command);

        await _repository.AddAsync(album, cancellationToken);

        return album.Id;
    }
}
