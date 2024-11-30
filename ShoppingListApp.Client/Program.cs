using System.Diagnostics;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShoppingListApp.Client;
using ShoppingListApp.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IProductService, HttpProductService>();
builder.Services.AddScoped<IShoppingListService, HttpShoppingListService>();

builder.Services
    .AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5100") });


await builder.Build().RunAsync();