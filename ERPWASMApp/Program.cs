using ERPWASMApp;
using ERPWASMApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
    new HttpClient{
        BaseAddress = new Uri("https://localhost:54528;http://localhost:54529")
    }
);
builder.Services.AddScoped<ERPAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<ERPAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();