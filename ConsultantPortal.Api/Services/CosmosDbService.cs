// Services/CosmosDbService.cs
using ConsultantPortal.Api.Models;
using Microsoft.Azure.Cosmos;


namespace ConsultantPortal.Api.Services;

public class CosmosDbService : ICosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(CosmosClient client, IConfiguration config)
    {
        var dbName = config["CosmosDb:DatabaseName"];
        var containerName = config["CosmosDb:ContainerName"];
        _container = client.GetContainer(dbName, containerName);
    }

    public async Task<TimeLog> AddTimeLogAsync(TimeLog log)
    {
        log.id ??= Guid.NewGuid().ToString();
        var response = await _container.CreateItemAsync(log, new PartitionKey(log.UserId));
        return response.Resource;
    }

    public async Task<IEnumerable<TimeLog>> GetTimeLogsAsync(string userId)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.UserId = @userId")
            .WithParameter("@userId", userId);

        var result = new List<TimeLog>();
        var iterator = _container.GetItemQueryIterator<TimeLog>(query);

        while (iterator.HasMoreResults)
        {
            var page = await iterator.ReadNextAsync();
            result.AddRange(page);
        }

        return result;
    }

    public async Task<TimeLog?> GetTimeLogByIdAsync(string id, string userId)
    {
        try
        {
            var response = await _container.ReadItemAsync<TimeLog>(id, new PartitionKey(userId));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task DeleteTimeLogAsync(string id, string userId)
    {
        await _container.DeleteItemAsync<TimeLog>(id, new PartitionKey(userId));
    }
}
