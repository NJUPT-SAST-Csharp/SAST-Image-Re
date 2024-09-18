using WebApp.Requests;

namespace WebApp.Utils;

public sealed class DataCollectionStorage<TItem>
{
    private readonly Queue<TItem> items = [];
    public int MaxCount { get; init; } = 50;

    public IEnumerable<TItem> Items => items;

    public int Count => items.Count;

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
}

public static class DataCollectionStorageConfiguration
{
    public static IServiceCollection AddDataCollectionStorages(this IServiceCollection services)
    {
        services.AddSingleton<DataCollectionStorage<DetailedAlbum>>();
        services.AddSingleton<DataCollectionStorage<ImageStreamWrapper>>();

        return services;
    }
}
