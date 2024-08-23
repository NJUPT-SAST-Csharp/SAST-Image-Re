using System.Diagnostics.CodeAnalysis;
using Domain.Extensions;

namespace Domain.Shared
{
    public sealed class EntityNotFoundException<TId>(TId id) : EntityNotFoundException
        where TId : IEquatable<TId>
    {
        public TId Id { get; } = id;

        [DoesNotReturn]
        public static void Throw(TId id) => throw new EntityNotFoundException<TId>(id);
    }

    public class EntityNotFoundException : DomainException
    {
        [DoesNotReturn]
        public static void Throw<TId>(TId id)
            where TId : IEquatable<TId> => throw new EntityNotFoundException<TId>(id);
    }
}
