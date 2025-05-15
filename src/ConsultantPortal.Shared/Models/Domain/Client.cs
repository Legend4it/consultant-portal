using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsultantPortal.Shared.Models.Domain;

public class Client
{
    [JsonProperty("id")] 
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Name { get; set; }
}
