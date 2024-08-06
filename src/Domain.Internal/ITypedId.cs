namespace Domain.Internal
{
    public interface ITypedId<TId> : IEquatable<TId>
    {
        public long Value { get; }

        public string ToString() => Value.ToString();

        public static abstract TId CreateNewId();
    }

    public interface ITypedId<TId, TValue> : IEquatable<TId>
        where TValue : IEquatable<TValue>
    {
        public TValue Value { get; }

        public string? ToString() => Value.ToString();

        public static abstract TId CreateNewId();
    }
}
