
using Newtonsoft.Json;

namespace ConsultantPortal.Api.Models;

public class Summary
{
    [JsonProperty("id")]
    public string Id { get; set; }=Guid.NewGuid().ToString();
    public object? Content { get; internal set; }
    public DateTime Created { get; internal set; }
}