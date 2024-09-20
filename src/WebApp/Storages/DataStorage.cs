namespace WebApp.Storages;

public interface IDataStorage<TItem>
{
    public void Add(TItem item);
    public void Clear();
    public bool TryGetValue(Func<TItem, bool> condition, out TItem? item);

    public IEnumerable<TItem> Items { get; }
    public int Count { get; }
    public bool IsEmpty => Count == 0;
    public bool HasValue => !IsEmpty;
}

public sealed class DataStorage<TItem> : IDataStorage<TItem>
{
    private readonly Queue<TItem> items = [];
    public IEnumerable<TItem> Items => items;
    public int Count => items.Count;
    public int MaxCount { get; init; } = int.MaxValue;

    public void Add(TItem item)
    {
        items.Enqueue(item);
        if (items.Count > MaxCount)
        {
            var removedItem = items.Dequeue();
            if (removedItem is IDisposable disposableItem)
                disposableItem.Dispose();
        }
    }

    public void Clear() => items.Clear();

    public bool TryGetValue(Func<TItem, bool> condition, out TItem? item)
    {
        if (Count == 0)
        {
            item = default;
            return false;
        }

        if (Items.Any(condition))
        {
            item = Items.First(condition);
            return true;
        }

        item = default;
        return false;
    }
}

public static class DataStorageConfiguration
{
    public static IServiceCollection AddDataStorage<TItem>(
        this IServiceCollection services,
        int maxCount = int.MaxValue
    )
    {
        services
            .AddSingleton(new DataStorage<TItem>() { MaxCount = maxCount })
            .AddSingleton<IDataStorage<TItem>>(p => p.GetRequiredService<DataStorage<TItem>>());

        return services;
    }

    public static IServiceCollection AddDataStorage<TItem, TStorage>(
        this IServiceCollection services
    )
        where TStorage : class, IDataStorage<TItem>, new()
    {
        services
            .AddSingleton<TStorage>()
            .AddSingleton<IDataStorage<TItem>>(p => p.GetRequiredService<TStorage>());

        return services;
    }
}
