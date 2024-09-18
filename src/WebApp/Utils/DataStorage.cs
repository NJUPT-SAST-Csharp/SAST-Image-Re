using System.Diagnostics.CodeAnalysis;
using WebApp.Requests;

namespace WebApp.Utils;

public sealed class DataStorage<T>
{
    [MemberNotNullWhen(true, nameof(Value))]
    public bool HasValue => Value is not null;
    public T? Value { get; set; } = default;
}

public static class DataStorageConfigurations
{
    public static IServiceCollection AddDataStorages(this IServiceCollection services)
    {
        services.AddSingleton<DataStorage<List<AlbumDto>>>();

        return services;
    }
}
