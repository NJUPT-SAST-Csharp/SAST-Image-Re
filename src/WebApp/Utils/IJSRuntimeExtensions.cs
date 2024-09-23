using Microsoft.JSInterop;

namespace WebApp.Utils;

public static class IJSRuntimeExtensions
{
    public static async Task SetSrcAsync(
        this IJSRuntime js,
        Stream image,
        string elementId,
        string? title = null
    )
    {
        image.Seek(0, SeekOrigin.Begin);
        using var strRef = new DotNetStreamReference(image, true);
        await js.InvokeVoidAsync("setSource", elementId, strRef, "image/*", title);
        //await image.DisposeAsync();
    }
}
