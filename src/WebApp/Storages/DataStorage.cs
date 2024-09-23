namespace WebApp.Storages;

public interface IDataStorage<TItem, TKey>
    where TKey : IEquatable<TKey>
{
    public bool TryGet(TKey key, out TItem? item);
    public void Add(TItem item, TKey key);
    public void Clear();

    public IReadOnlyDictionary<TKey, TItem> Items { get; }
    public int Count { get; }
    public bool IsEmpty => Count == 0;
    public bool HasValue => !IsEmpty;
}

file sealed class DataStorage<TItem, TKey> : IDataStorage<TItem, TKey>
    where TKey : IEquatable<TKey>
{
    private readonly Dictionary<TKey, TItem> items = [];

    public IReadOnlyDictionary<TKey, TItem> Items => items;
    public int Count => items.Count;

    public void Add(TItem item, TKey key)
    {
        if (items.TryAdd(key, item) == false)
        {
            TryGet(key, out var existingValue);
            if (key is IEquatable<TItem> comparableItem && comparableItem.Equals(existingValue))
            {
                return;
            }

            items.Remove(key);
            items.Add(key, item);
        }
    }

    public void Clear() => items.Clear();

    public bool TryGet(TKey key, out TItem? item)
    {
        return items.TryGetValue(key, out item);
    }
}

public static class DataStorageConfiguration
{
    public static IServiceCollection AddDataStorage<TItem, TKey>(this IServiceCollection services)
        where TKey : IEquatable<TKey>
    {
        services.AddSingleton<IDataStorage<TItem, TKey>>(new DataStorage<TItem, TKey>());

        return services;
    }

    public static IServiceCollection AddDataStorage<TItem, TKey, TStorage>(
        this IServiceCollection services
    )
        where TKey : IEquatable<TKey>
        where TStorage : class, IDataStorage<TItem, TKey>, new()
    {
        services.AddSingleton<IDataStorage<TItem, TKey>>(new TStorage());

        return services;
    }
}
