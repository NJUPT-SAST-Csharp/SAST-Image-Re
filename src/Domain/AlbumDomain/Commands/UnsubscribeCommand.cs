using Domain.AlbumDomain.AlbumEntity;
using Domain.Command;
using Domain.Extensions;
using Domain.Shared;

namespace Domain.AlbumDomain.Commands
{
    public sealed record class UnsubscribeCommand(AlbumId Album, Actor Actor) : IDomainCommand { }

    internal sealed class UnsubscribeCommandHandler(IRepository<Album, AlbumId> repository)
        : ICommandHandler<UnsubscribeCommand>
    {
        private readonly IRepository<Album, AlbumId> _repository = repository;

        public async Task Handle(UnsubscribeCommand request, CancellationToken cancellationToken)
        {
            var album = await _repository.GetAsync(request.Album, cancellationToken);

            album.Unsubscribe(request);
        }
    }
}
