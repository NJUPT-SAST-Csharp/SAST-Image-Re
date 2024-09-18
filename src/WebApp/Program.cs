using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebApp;
using WebApp.APIs;
using WebApp.Utils;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddApiClients().AddAuth();
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddDataStorages().AddDataCollectionStorages();

await builder.Build().RunAsync();
