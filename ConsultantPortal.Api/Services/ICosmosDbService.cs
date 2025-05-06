using ConsultantPortal.Api.Models;

namespace ConsultantPortal.Api.Services;

public interface ICosmosDbService
{
    Task<TimeLog> AddTimeLogAsync(TimeLog log);
    Task<IEnumerable<TimeLog>> GetTimeLogsAsync(string userId);
    Task<TimeLog?> GetTimeLogByIdAsync(string id, string userId);
    Task DeleteTimeLogAsync(string id, string userId);
}
