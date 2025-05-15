// Services/CosmosDbService.cs
using ConsultantPortal.Shared.Models.Abstractions;
using Microsoft.Azure.Cosmos;

namespace ConsultantPortal.Api.Services;

public class CosmosDbService<T> : ICosmosDbService<T> where T : class
{
    private readonly Container _container;

    public CosmosDbService(CosmosClient client, string databaseName, string containerName)
    {
        _container = client.GetContainer(databaseName, containerName);
    }
    public async Task<IEnumerable<T>> GetItemsAsync(string query)
    {
        var iterator = _container.GetItemQueryIterator<T>(new QueryDefinition(query));
        var results = new List<T>();
        while (iterator.HasMoreResults)
        {
            results.AddRange(await iterator.ReadNextAsync());
        }
        return results;
    }
    public async Task<T?> GetItemAsync(string id)
    {
        try
        {
            var response = await _container.ReadItemAsync<T>(id, PartitionKey.None);
            return response;
        }
        catch (CosmosException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }
    public async Task<T> CreateItemAsync(T item) => (await _container.CreateItemAsync<T>(item)).Resource;

    public async Task<T> UpdateItemAsync(string id, T item) => (await _container.UpsertItemAsync(item, PartitionKey.None)).Resource;


    public async Task<bool> DeleteItemAsync(string id)
    {
        try
        {
            await _container.DeleteItemAsync<T>(id, PartitionKey.None);
            return true;
        }
        catch (CosmosException e) when (e.StatusCode != System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }



}
