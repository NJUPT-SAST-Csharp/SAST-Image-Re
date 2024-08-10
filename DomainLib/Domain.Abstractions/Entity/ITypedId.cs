namespace Domain.Entity
{
    public interface ITypedId<TId, TValue> : IEquatable<TId>, IBaseTypedId
        where TValue : IEquatable<TValue>
    {
        public TValue Value { get; }

        public static abstract TId GenerateNew();
    }

    public interface ITypedId<TId, TValue, TInput> : IEquatable<TId>, IBaseTypedId
        where TValue : IEquatable<TValue>
    {
        public static abstract TId GenerateNew(TInput requirement);
    }

    public interface IBaseTypedId
    {
        public string ToString();
    }
}
