using Domain.Internal.Command;

namespace Domain.AlbumEntity.Commands.CreateAlbum
{
    internal class CreateAlbumCommandHandler : IAsyncDomainCommandHandler<CreateAlbumCommand>
    {
        public Task HandleAsync(
            ref readonly CreateAlbumCommand command,
            CancellationToken cancellationToken = default
        ) { }
    }
}
