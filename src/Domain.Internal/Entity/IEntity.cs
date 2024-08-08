namespace Domain.Internal
{
    public interface IEntity<T>
        where T : IEquatable<T>
    {
        public T Id { get; }
    }
}
