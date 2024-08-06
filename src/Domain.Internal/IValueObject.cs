namespace Domain.Internal
{
    public interface IValueObject<TObject, TValue> : IEquatable<TObject>
    {
        public TValue Value { get; }
        public static abstract TObject CreateNew(TValue value);
    }
}
