using Domain.Command;
using Domain.Extensions;
using Domain.Shared;
using Domain.TagDomain.Services;
using Domain.TagDomain.TagEntity;

namespace Domain.TagDomain.Commands
{
    public sealed record CreateTagCommand(TagName Name, Actor Actor) : IDomainCommand<TagId> { }

    internal sealed class CreateTagCommandHandler(
        IRepository<Tag, TagId> repository,
        ITagNameUniquenessChecker checker
    ) : IDomainCommandHandler<CreateTagCommand, TagId>
    {
        private readonly IRepository<Tag, TagId> _repository = repository;
        private readonly ITagNameUniquenessChecker _checker = checker;

        public async Task<TagId> Handle(
            CreateTagCommand command,
            CancellationToken cancellationToken
        )
        {
            await _checker.CheckAsync(command.Name, cancellationToken);

            var tag = new Tag(command);

            var id = await _repository.AddAsync(tag, cancellationToken);

            return id;
        }
    }
}
