using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EventEaseApp;
using Blazored.LocalStorage;
using EventEase.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<EventStorageService>();
builder.Services.AddScoped<UserSessionService>();
builder.Services.AddScoped<AttendanceService>();


var app = builder.Build();

var sessionService = app.Services.GetRequiredService<UserSessionService>();
await sessionService.InitializeAsync();

await app.RunAsync();
