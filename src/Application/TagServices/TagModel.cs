using Domain.TagDomain.Events;

namespace Application.TagServices
{
    public sealed class TagModel
    {
        private TagModel() { }

        public TagModel(TagCreatedEvent e)
        {
            Id = e.Id.Value;
            Name = e.TagName.Value;
        }

        public long Id { get; }
        public string Name { get; } = null!;
    }
}
