using Domain.TagDomain.Events;

namespace Application.TagServices;

public sealed class TagModel
{
    private TagModel() { }

    public long Id { get; }
    public string Name { get; private set; } = null!;

    public TagModel(TagCreatedEvent e)
    {
        Id = e.Id.Value;
        Name = e.TagName.Value;
    }

    public void Update(TagUpdatedEvent e)
    {
        Name = e.NewName.Value;
    }
}
