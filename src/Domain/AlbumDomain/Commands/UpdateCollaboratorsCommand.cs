using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Services;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands
{
    public readonly record struct UpdateCollaboratorsCommand(
        AlbumId Album,
        Collaborators Collaborators,
        Actor Actor
    ) : IDomainCommand { }

    internal sealed class UpdateCollaboratorsCommandHandler(
        IRepository<Album, AlbumId> repository,
        ICollaboratorsExistenceChecker checker
    ) : ICommandHandler<UpdateCollaboratorsCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;
        private readonly ICollaboratorsExistenceChecker _checker = checker;

        public async Task Handle(
            UpdateCollaboratorsCommand request,
            CancellationToken cancellationToken
        )
        {
            await _checker.CheckAsync(request.Collaborators, cancellationToken);

            var album = await _repository.GetAsync(request.Album, cancellationToken);

            album.UpdateCollaborators(in request);
        }
    }
}
