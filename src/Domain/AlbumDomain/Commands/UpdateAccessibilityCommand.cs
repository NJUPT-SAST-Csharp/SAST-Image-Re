using Domain.AlbumDomain.AlbumEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands
{
    public readonly record struct UpdateAccessibilityCommand(
        AlbumId Album,
        Accessibility Accessibility,
        Actor Actor
    ) : IDomainCommand { }

    internal sealed class UpdateAccessibilityCommandHandler(IRepository<Album, AlbumId> repository)
        : ICommandHandler<UpdateAccessibilityCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;

        public async Task Handle(
            UpdateAccessibilityCommand request,
            CancellationToken cancellationToken
        )
        {
            var album = await _repository.GetAsync(request.Album, cancellationToken);

            album.UpdateAccessibility(in request);
        }
    }
}
