namespace WebApp.Storages;

public interface IKeyDataStorage<TItem, TKey>
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

public sealed class KeyDataStorage<TItem, TKey> : IKeyDataStorage<TItem, TKey>
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

public static class KeyDataStorageConfiguration
{
    public static IServiceCollection AddKeyDataStorage<TItem, TKey>(
        this IServiceCollection services
    )
        where TKey : IEquatable<TKey>
    {
        services
            .AddSingleton(new KeyDataStorage<TItem, TKey>())
            .AddSingleton<IKeyDataStorage<TItem, TKey>>(p =>
                p.GetRequiredService<KeyDataStorage<TItem, TKey>>()
            );

        return services;
    }

    public static IServiceCollection AddKeyDataStorage<TItem, TKey, TStorage>(
        this IServiceCollection services
    )
        where TKey : IEquatable<TKey>
        where TStorage : class, IKeyDataStorage<TItem, TKey>, new()
    {
        services
            .AddSingleton<TStorage>()
            .AddSingleton<IKeyDataStorage<TItem, TKey>>(p => p.GetRequiredService<TStorage>());

        return services;
    }
}
