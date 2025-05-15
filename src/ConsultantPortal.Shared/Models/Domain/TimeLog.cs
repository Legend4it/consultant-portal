using System.Data;
using Newtonsoft.Json;

namespace ConsultantPortal.Shared.Models.Domain;

public class TimeLog
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? ProjectId { get; set; }
    public string? ClientId { get; set; }
    public DateTime? Date { get; set; }
    public double Hours { get; set; }
    public string? Description { get; set; }
}
