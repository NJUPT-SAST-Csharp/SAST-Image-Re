using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Toolbelt.Blazor.I18nText;
using WebApp;
using WebApp.APIs;
using WebApp.Requests;
using WebApp.Storages;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder
    .Services.AddMasaBlazor()
    .AddI18nForWasmAsync($"{builder.HostEnvironment.BaseAddress}/i18n");

builder.Services.AddApiClients().AddAuth();
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder
    .Services.AddKeyDataStorage<AlbumItemDto, long>()
    .AddKeyDataStorage<DetailedAlbum, long>()
    .AddKeyDataStorage<Stream, long>();
builder.Services.AddScoped(_ => new HttpClient()
{
    BaseAddress = new(builder.HostEnvironment.BaseAddress),
});
builder.Services.AddI18nText(options =>
    options.PersistenceLevel = PersistanceLevel.SessionAndLocal
);

await builder.Build().RunAsync();
