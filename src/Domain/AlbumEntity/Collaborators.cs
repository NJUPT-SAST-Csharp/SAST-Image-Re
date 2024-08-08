using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Domain.Internal.Entity;
using Domain.UserEntity;

namespace Domain.AlbumEntity
{
    public sealed class Collaborators
        : ReadOnlyCollection<UserId>,
            IValueObject<Collaborators, IReadOnlyCollection<UserId>, IEnumerable<UserId>>
    {
        public const int MaxCount = 32;

        private Collaborators(IList<UserId> list)
            : base(list) { }

        public Collaborators()
            : this([]) { }

        public static bool TryCreateNew(
            IEnumerable<UserId> value,
            [MaybeNullWhen(false), NotNullWhen(true)] out Collaborators? newObject
        )
        {
            value = value.Distinct();

            if (value.Count() > MaxCount)
            {
                newObject = null;
                return false;
            }

            newObject = new(value.ToList());
            return true;
        }

        public IReadOnlyCollection<UserId> Value => Items.AsReadOnly();

        public bool Equals(Collaborators? other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Items.SequenceEqual(other.Items);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Collaborators);
        }

        public override int GetHashCode()
        {
            return Items.GetHashCode();
        }
    }
}
