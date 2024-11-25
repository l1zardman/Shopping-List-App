using System.Diagnostics;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShoppingListApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5100") });

Console.WriteLine("---------------------------------");
Console.WriteLine(builder.Configuration["ApiUrl"]);
Debug.WriteLine(builder.Configuration["ApiUrl"]);
Console.WriteLine("---------------------------------");

await builder.Build().RunAsync();