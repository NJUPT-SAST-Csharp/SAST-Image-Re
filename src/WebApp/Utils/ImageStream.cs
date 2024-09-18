namespace WebApp.Utils;

public readonly record struct ImageStreamWrapper(long Id, Stream Image)
    : IDisposable,
        IAsyncDisposable
{
    public void Dispose()
    {
        Image.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return Image.DisposeAsync();
    }
}
