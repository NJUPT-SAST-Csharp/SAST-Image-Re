using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Domain.Entity;
using Domain.TagDomain.TagEntity;

namespace Domain.AlbumDomain.ImageEntity;

public sealed class ImageTags
    : ReadOnlyCollection<TagId>,
        IValueObject<ImageTags, IReadOnlyCollection<TagId>>,
        IFactoryConstructor<ImageTags, long[]?>
{
    public const int MaxCount = 10;

    internal ImageTags(IList<TagId> list)
        : base(list) { }

    public ImageTags()
        : this([]) { }

    public IReadOnlyCollection<TagId> Value => Items.AsReadOnly();

    public static bool TryCreateNew(long[]? input, [NotNullWhen(true)] out ImageTags? entity)
    {
        if (input is null || input.Length == 0)
        {
            entity = [];
            return true;
        }

        var mid = input.Distinct();

        if (mid.Count() > MaxCount)
        {
            entity = null;
            return false;
        }

        var tagIds = mid.Select(t => new TagId(t)).ToList();

        entity = new(tagIds);
        return true;
    }

    public bool Equals(ImageTags? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Items.SequenceEqual(other.Items);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ImageTags);
    }

    public override int GetHashCode()
    {
        return Items.GetHashCode();
    }
}
