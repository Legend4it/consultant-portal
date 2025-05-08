using Azure.AI.OpenAI;
using ConsultantPortal.Api.Models;
using ConsultantPortal.Api.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                settings.DatabaseName,
                containerName
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
