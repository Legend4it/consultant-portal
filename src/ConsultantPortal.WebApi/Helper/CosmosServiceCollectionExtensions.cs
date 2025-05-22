using Azure.AI.OpenAI;
using ConsultantPortal.Api.Services;
using ConsultantPortal.Shared.Models.Abstractions;
using ConsultantPortal.Shared.Models.Domain;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System.ClientModel;


namespace ConsultantPortal.Api.Helper;

public static class CosmosServiceCollectionExtensions
{
    public static IServiceCollection AddCosmosService<T>(
        this IServiceCollection services)
        where T : class
    {
        services.AddSingleton<ICosmosDbService<T>>(sp =>
        {
            var client = sp.GetRequiredService<CosmosClient>();
            var settings = sp.GetRequiredService<CosmosDbSettings>();

            var containerName =
                $"{settings.ContainerPrefix}-{typeof(T).Name.ToLowerInvariant()}";

            return new CosmosDbService<T>(
                client,
                settings.DatabaseName!,
                settings.ContainerName!
            );
        });

        return services;
    }
    public static WebApplicationBuilder AddAzureOpenAIClient(this WebApplicationBuilder services)
    {
        services.Services.Configure<AzureOpenAISettings>(
            services.Configuration.GetSection("AzureOpenAI"));

        services.Services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<AzureOpenAISettings>>().Value);

        services.Services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<AzureOpenAISettings>();
            return new AzureOpenAIClient(
                new Uri(settings.Endpoint),
                new ApiKeyCredential(settings.ApiKey)
            );
        });

        return services;
    }
}
