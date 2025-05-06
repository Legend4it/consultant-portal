using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using ConsultantPortal.Api.Models;
using ConsultantPortal.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Load Configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.Configure<CosmosDbSettings>(
    builder.Configuration.GetSection("CosmosDb")
);

// Register CosmosClient with DI
builder.Services.AddSingleton<CosmosClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<CosmosDbSettings>>().Value;
    return new CosmosClient(settings.Account, settings.Key);
});


// Register Services
builder.Services.AddSingleton<ICosmosDbService, CosmosDbService>();
builder.Services.AddSingleton<CosmosDbInitializer>();


var app = builder.Build();

// Initialize Cosmos DB on startup
using (var scope = app.Services.CreateScope())
{
    var cosmosDbInitializer = scope.ServiceProvider.GetRequiredService<CosmosDbInitializer>();
    await cosmosDbInitializer.InitializeAsync();
}

// Middleware for error handling
app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
});

// minimal API for TimeLog
var timeLogGroup = app.MapGroup("/api/timelogs").WithTags("TimeLogs"); // For grouping in Swagger

timeLogGroup.MapGet("/{userId}", async (string userId, ICosmosDbService db) =>
{
    var logs = await db.GetTimeLogsAsync(userId);
    return Results.Ok(logs);
});

timeLogGroup.MapPost("/", async (TimeLog log, ICosmosDbService db) =>
{
    var saved = await db.AddTimeLogAsync(log);
    return Results.Created($"/api/timelogs/{saved.UserId}/{saved.id}", saved);
});

timeLogGroup.MapDelete("/{userId}/{id}", async (string userId, string id, ICosmosDbService db) =>
{
    await db.DeleteTimeLogAsync(id, userId);
    return Results.NoContent();
});


app.Run();



