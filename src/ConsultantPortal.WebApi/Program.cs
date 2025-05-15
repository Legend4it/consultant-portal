using ConsultantPortal.Shared.Models.Domain;
using ConsultantPortal.Api.Services;
using ConsultantPortal.Api.Helper;

var builder = WebApplication.CreateBuilder(args);



builder.AddCosmosDbService()
    .AddAzureOpenAIClientService()
    .BuildServices(service => service
        .AddCosmosService<TimeLog>()
        .AddCosmosService<Project>()
        .AddCosmosService<Client>()
        .AddCosmosService<Summary>()
        );



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


app.MapAllEndpoints();

app.Run();



