using System.Diagnostics.CodeAnalysis;

namespace Domain.Internal.Entity
{
    public interface IValueObject<TObject, TValue> : IValueObject<TObject, TValue, TValue> { }

    public interface IValueObject<TObject, TValue, TInput> : IEquatable<TObject>
    {
        public TValue Value { get; }
        public static abstract bool TryCreateNew(
            TInput value,
            [MaybeNullWhen(false), NotNullWhen(true)] out TObject? newObject
        );
    }
}
