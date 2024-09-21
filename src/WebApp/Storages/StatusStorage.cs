namespace WebApp.Storages;

public interface IStatusStorage<TStatus> : IDisposable
{
    public TStatus Value { get; set; }

    public event Action? OnChange;
}

public sealed class StatusStorage<TStatus>(TStatus status) : IStatusStorage<TStatus>
{
    public TStatus Value
    {
        get => status!;
        set
        {
            status = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;

    public void Dispose()
    {
        OnChange = null;
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}

public static class StatusStorageConfiguration
{
    public static IServiceCollection AddStatusStorage<TStatus>(
        this IServiceCollection services,
        TStatus defaultValue = default!
    )
    {
        services
            .AddSingleton<IStatusStorage<TStatus>>(new StatusStorage<TStatus>(defaultValue))
            .AddCascadingValue(sp => sp.GetRequiredService<IStatusStorage<TStatus>>());

        return services;
    }

    public static IServiceCollection AddKeyedStatusStorage<TStatus>(
        this IServiceCollection services,
        string key,
        TStatus defaultValue = default!
    )
    {
        services
            .AddKeyedSingleton<IStatusStorage<TStatus>>(
                key,
                new StatusStorage<TStatus>(defaultValue)
            )
            .AddCascadingValue(key, sp => sp.GetRequiredKeyedService<IStatusStorage<TStatus>>(key));

        return services;
    }
}
