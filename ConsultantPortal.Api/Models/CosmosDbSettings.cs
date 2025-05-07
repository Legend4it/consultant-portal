namespace ConsultantPortal.Api.Models;

public class CosmosDbSettings
{
    public string AccountUri { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = "ConsultantDB";
    public string ContainerName { get; set; } = "TimeLogs";
}