using Achiever.Client.Components;
using Achiever.Client.Services.Goals;
using Achiever.Client.Services.Identity;
using Achiever.Client.Services.Snapshot;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// register the cookie handler
builder.Services.AddScoped<CookieHandler>();

// set up authorization
builder.Services.AddAuthorizationCore();

// register the custom state provider
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

// register the account management interface
builder.Services.AddScoped(
    sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

// set base address for default host
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "https://localhost:5002") });

// configure client for auth interactions
builder.Services.AddHttpClient(
    "Auth",
    opt => opt.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:5001"))
    .AddHttpMessageHandler<CookieHandler>();

builder.Services.AddHttpClient<GoalClient>("Goals", opt => opt.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:5001"));
    //.AddHttpMessageHandler<CookieHandler>();

builder.Services.AddFluentUIComponents();

builder.Services.AddScoped<ISnapshotService, SnapshotService>();

await builder.Build().RunAsync();
