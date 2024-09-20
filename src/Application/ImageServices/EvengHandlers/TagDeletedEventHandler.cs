using Domain.Core.Event;
using Domain.TagDomain.Events;

namespace Application.ImageServices.EvengHandlers;

internal sealed class TagDeletedEventHandler(IImageModelTagDeletedRepository repository)
    : IDomainEventHandler<TagDeletedEvent>
{
    private readonly IImageModelTagDeletedRepository _repository = repository;

    public async Task Handle(TagDeletedEvent e, CancellationToken cancellationToken)
    {
        var images = await _repository.GetAsync(e.Id, cancellationToken);

        foreach (var image in images)
        {
            image.DeleteTag(e);
        }
    }
}
