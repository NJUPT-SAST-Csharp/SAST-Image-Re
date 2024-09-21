namespace WebApp.Storages;

public interface IStatusStorage<TStatus>
{
    public TStatus Value { get; set; }
}

public sealed class StatusStorage<TStatus> : IStatusStorage<TStatus>
{
    public required TStatus Value { get; set; }
}

public static class DataStorageConfiguration
{
    public static IServiceCollection AddStatusStorage<TStatus>(
        this IServiceCollection services,
        TStatus defaultValue = default!
    )
    {
        services
            .AddSingleton(new StatusStorage<TStatus>() { Value = defaultValue })
            .AddSingleton<IStatusStorage<TStatus>>(p =>
                p.GetRequiredService<StatusStorage<TStatus>>()
            );

        return services;
    }

    public static IServiceCollection AddKeyedStatusStorage<TStatus>(
        this IServiceCollection services,
        string key,
        TStatus defaultValue = default!
    )
    {
        services
            .AddKeyedSingleton(key, new StatusStorage<TStatus>() { Value = defaultValue })
            .AddKeyedSingleton<IStatusStorage<TStatus>>(
                key,
                (p, k) => p.GetRequiredKeyedService<StatusStorage<TStatus>>(k)
            );

        return services;
    }
}
