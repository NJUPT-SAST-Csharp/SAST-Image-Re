using Blazored.LocalStorage;
using Masa.Blazor;
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
    .Services.AddMasaBlazor(options =>
    {
        options.ConfigureBreakpoint(breakpoints =>
        {
            breakpoints.Thresholds = new()
            {
                Xs = 576,
                Sm = 768,
                Md = 992,
                Lg = 1200,
            };
        });
    })
    .AddI18nForWasmAsync($"{builder.HostEnvironment.BaseAddress}/i18n");

builder
    .Services.AddAuth()
    .AddApiClient<IAlbumAPI>(IAlbumAPI.Base)
    .AddApiClient<IAccountAPI>(IAccountAPI.Base)
    .AddApiClient<IUserAPI>(IUserAPI.Base)
    .AddApiClient<IImageAPI>(IImageAPI.Base);

builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddDataStorage<AlbumItemDto, long>().AddDataStorage<DetailedAlbum, long>();

builder
    .Services.AddStatusStorage<AuthState>()
    .AddStatusStorage<ExceptionRequest>()
    .AddKeyedStatusStorage("loading", false)
    .AddKeyedStatusStorage("avatar", string.Empty);

builder.Services.AddI18nText(options =>
    options.PersistenceLevel = PersistanceLevel.SessionAndLocal
);

await builder.Build().RunAsync();
