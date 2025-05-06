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
        var dbName = _config["CosmosDb:DatabaseName"];
        var containerName = _config["CosmosDb:ContainerName"];
        var partitionKey = "/UserId";

        var database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(dbName);
        await database.Database.CreateContainerIfNotExistsAsync(
            new ContainerProperties(containerName, partitionKey));
    }
}