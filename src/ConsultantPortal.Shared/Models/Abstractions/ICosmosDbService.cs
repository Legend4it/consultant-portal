
namespace ConsultantPortal.Shared.Models.Abstractions;

public interface ICosmosDbService<T>
{
    Task<IEnumerable<T>> GetItemsAsync(string query);
    Task<T?> GetItemAsync(string id);
    Task<T> CreateItemAsync(T item);
    Task<T> UpdateItemAsync(string id, T item);
    Task<bool> DeleteItemAsync(string id);
}
