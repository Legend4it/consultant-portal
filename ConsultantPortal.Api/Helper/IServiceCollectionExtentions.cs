using ConsultantPortal.Api.Models;
using ConsultantPortal.Api.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace ConsultantPortal.Api.Helper;

internal static class IServiceCollectionExtentions
{
    internal static IServiceCollection RegisterCosmosDbService(this IServiceCollection services)
    {
        services.AddSingleton<ICosmosDbService, CosmosDbService>();
        services.AddSingleton<CosmosDbInitializer>();
        return services;
    }

    internal static IServiceCollection RegisterCosmosDbClient(this IServiceCollection services)
    {
        IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>()!;

        services.Configure<CosmosDbSettings>(configuration.GetSection("CosmosDb"));
        return services.AddSingleton<CosmosClient>(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<CosmosDbSettings>>().Value;
            return new CosmosClient(settings.AccountUri, settings.Key);
        });
    }
}
