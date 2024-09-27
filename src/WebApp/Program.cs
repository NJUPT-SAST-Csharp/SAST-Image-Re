using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Toolbelt.Blazor.I18nText;
using WebApp;
using WebApp.APIs;
using WebApp.APIs.Auth;
using WebApp.Requests;
using WebApp.Storages;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder
    .Services.AddMasaBlazor()
    .AddI18nForWasmAsync($"{builder.HostEnvironment.BaseAddress}/i18n");

builder
    .Services.AddAuth()
    .AddApiClient<IAlbumAPI>(IAlbumAPI.Base)
    .AddApiClient<IAccountAPI>(IAccountAPI.Base)
    .AddApiClient<IImageAPI>(IImageAPI.Base);

builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddDataStorage<AlbumItemDto, long>().AddDataStorage<DetailedAlbum, long>();

builder
    .Services.AddStatusStorage<AuthState>()
    .AddStatusStorage<ExceptionRequest>()
    .AddKeyedStatusStorage("loading", false);

builder.Services.AddI18nText(options =>
    options.PersistenceLevel = PersistanceLevel.SessionAndLocal
);

await builder.Build().RunAsync();
