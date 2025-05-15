using ConsultantPortal.Shared.Models.Domain;
using Microsoft.Azure.Cosmos;

namespace ConsultantPortal.Api.Services;
public class CosmosDbInitializer
{
    private readonly CosmosClient _cosmosClient;
    private readonly IConfiguration _config;


    public CosmosDbInitializer(CosmosClient cosmosClient, IConfiguration configuration)
    {
        _cosmosClient = cosmosClient;
        _config = configuration;
    }
    



    public async Task InitializeAsync()
    {
        var settings = _config.GetSection("CosmosDb").Get<CosmosDbSettings>()!;
        var dbName = settings.DatabaseName;
        var prefix = settings.ContainerPrefix;
        var partitionKey = "/id";

        await _cosmosClient.CreateDatabaseIfNotExistsAsync(dbName);
        var db = _cosmosClient.GetDatabase(dbName);

        Type[] entityTypes = {
        typeof(TimeLog),
        typeof(Project),
        typeof(Client),
        typeof(Summary)};

        foreach (var type in entityTypes)
        {
            var containerName = $"{prefix}-{type.Name.ToLowerInvariant()}";
            await db.CreateContainerIfNotExistsAsync(new ContainerProperties
            {
                Id = containerName,
                PartitionKeyPath = partitionKey
            });
        }
    }
}