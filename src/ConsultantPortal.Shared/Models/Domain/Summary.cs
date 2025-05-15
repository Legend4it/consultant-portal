
using Newtonsoft.Json;

namespace ConsultantPortal.Shared.Models.Domain;

public class Summary
{
    [JsonProperty("id")]
    public string Id { get; set; }=Guid.NewGuid().ToString();
    public object? Content { get; set; }
    public DateTime Created { get; set; }
}