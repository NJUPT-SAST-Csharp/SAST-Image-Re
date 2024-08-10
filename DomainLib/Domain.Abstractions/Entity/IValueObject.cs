namespace Domain.Entity
{
    public interface IValueObject<TObject, TValue> : IEquatable<TObject>
    {
        public TValue Value { get; }
    }
}
