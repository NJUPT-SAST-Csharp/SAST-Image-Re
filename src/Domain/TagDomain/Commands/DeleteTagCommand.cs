using Domain.Command;
using Domain.Extensions;
using Domain.Shared;
using Domain.TagDomain.TagEntity;

namespace Domain.TagDomain.Commands
{
    public sealed record DeleteTagCommand(TagId Id, Actor Actor) : IDomainCommand { }

    internal sealed class DeleteTagCommandHandler(IRepository<Tag, TagId> repository)
        : IDomainCommandHandler<DeleteTagCommand>
    {
        private readonly IRepository<Tag, TagId> _repository = repository;

        public Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            if (request.Actor.IsAdmin == false)
                throw new NoPermissionException();

            return _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
