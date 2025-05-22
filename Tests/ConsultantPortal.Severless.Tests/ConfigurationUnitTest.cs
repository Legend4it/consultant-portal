using ConsultantPortal.Shared.Models.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ConsultantPortal.Severless.Tests;

public class ConfigurationUnitTest
{
    [Fact]
    public void CosmosDbSettings_Binds_From_Configuration()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            ["CosmosDb:Endpoint"] = "https://localhost:8081",
            ["CosmosDb:Key"] = "superSecretKey",
            ["CosmosDb:DatabaseName"] = "MyTestDb",
        };

        var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(cfg =>
            {
                cfg.Sources.Clear();
                cfg.AddInMemoryCollection(inMemorySettings!);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddOptions<CosmosDbSettings>()
                        .BindConfiguration("CosmosDb");
            })
            .Build();

        var opts = host.Services.GetRequiredService<IOptions<CosmosDbSettings>>();
        var settings = opts.Value;

        Assert.Equal("https://localhost:8081", settings.Endpoint);
        Assert.Equal("superSecretKey", settings.Key);
        Assert.Equal("MyTestDb", settings.DatabaseName);
    }
}