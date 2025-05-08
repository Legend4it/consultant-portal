namespace ConsultantPortal.Api.Models;

public class CosmosDbSettings
{
    public string Endpoint { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string? DatabaseName { get; set; }
    public string? ContainerName { get; set; }
    public string? ContainerPrefix { get; set; }
}