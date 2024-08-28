using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Domain.Entity;
using Domain.UserDomain.UserEntity;

namespace Domain.AlbumDomain.AlbumEntity
{
    public sealed class Collaborators
        : ReadOnlyCollection<UserId>,
            IValueObject<Collaborators, IReadOnlyCollection<UserId>>,
            IFactoryConstructor<Collaborators, long[]>
    {
        public const int MaxCount = 32;

        internal Collaborators(IList<UserId> list)
            : base(list) { }

        public Collaborators()
            : this([]) { }

        public static bool TryCreateNew(
            long[] value,
            [MaybeNullWhen(false), NotNullWhen(true)] out Collaborators? newObject
        )
        {
            var mid = value.Distinct();

            if (mid.Count() > MaxCount)
            {
                newObject = null;
                return false;
            }

            var userIds = mid.Select(id => new UserId(id)).ToList();

            newObject = new(userIds);
            return true;
        }

        public bool NotContains(UserId userId) => !Contains(userId);

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
