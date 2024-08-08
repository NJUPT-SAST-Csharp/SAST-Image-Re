namespace Domain.Internal.Entity
{
    public interface ITypedId<TId> : ITypedId<TId, long> { }

    public interface ITypedId<TId, TValue> : IEquatable<TId>
        where TValue : IEquatable<TValue>
    {
        public TValue Value { get; }

        public string? ToString() => Value.ToString();

        public static abstract TId CreateNewId();
    }
}
