using System.Security.Claims;
using Achiever.Infrastructure.Database;
using Achiever.Infrastructure.Endpoints;
using Achiever.Services.Goals.Domain;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// cookie authentication
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// configure authorization
builder.Services.AddAuthorizationBuilder();

// add the database (in memory for the sample)
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IGoalReadRepository, GoalService>();
builder.Services.AddScoped<IGoalWriteRepository, GoalService>();
builder.Services.AddSingleton<IAccountContext, AccountContext>();

// add identity and opt-in to endpoints
builder.Services.AddIdentityCore<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

// add CORS policy for Wasm client
builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins([builder.Configuration["BackendUrl"] ?? "https://localhost:5001", 
            builder.Configuration["FrontendUrl"] ?? "https://localhost:5002"])
            .AllowAnyMethod()
            .SetIsOriginAllowed(pol => true)
            .SetIsOriginAllowedToAllowWildcardSubdomains()
            .AllowAnyHeader()
            .AllowCredentials()));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at
// https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpoints();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();


var app = builder.Build();

// create routes for the identity endpoints
app.MapIdentityApi<AppUser>();

app.MapPost("/Logout", async (SignInManager<AppUser> signInManager, [FromBody] object empty) =>
{
    if (empty != null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
    return Results.Unauthorized();
}).RequireAuthorization();

// activate the CORS policy
app.UseCors("wasm");

app.UseAuthentication();

app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var allEndpointsGroup = app.MapGroup("");
allEndpointsGroup.WithOpenApi();

var scope = app.Services.CreateScope();

var endpoints = scope.ServiceProvider.GetServices<IEndpoint>();
foreach (var endpoint in endpoints)
{
    endpoint.Map(allEndpointsGroup);
}

try
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var id = await LocalUserSeeder.SeedLocalUserIfNotExists(userManager);
}
catch (Exception ex)
{
    Console.WriteLine("An error occurred while seeding the database.", ex);
}

app.Run();

public partial class Program() { }