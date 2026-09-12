using Microsoft.AspNetCore.Components;
using TaskTracker.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Swagger: generates the OpenAPI document and serves the Swagger UI page
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// OPTIONAL bonus: Blazor page (server-side interactive) that calls the API
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// HttpClient used by the Blazor page to call this same API.
// BaseAddress is taken from the current page URL (e.g. http://localhost:5041/), so no port is hard-coded.
builder.Services.AddScoped(sp =>
{
    var navigation = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigation.BaseUri) };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAntiforgery();   // required by Blazor's form handling

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
