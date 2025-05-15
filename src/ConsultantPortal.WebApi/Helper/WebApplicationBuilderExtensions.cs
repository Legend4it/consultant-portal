using Azure.AI.OpenAI;
using ConsultantPortal.Shared.Models.Domain;
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

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddCosmosDbService(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<CosmosDbSettings>(
            builder.Configuration.GetSection("CosmosDb"));

        builder.Services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<CosmosDbSettings>>().Value);

        builder.Services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<CosmosDbSettings>();
            return new CosmosClient(settings.Endpoint, settings.Key, new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            });
        });

        builder.Services.AddSingleton<CosmosDbInitializer>();

        return builder;
    }
    public static WebApplicationBuilder AddAzureOpenAIClientService(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AzureOpenAISettings>(
            builder.Configuration.GetSection("OpenAI"));

        builder.Services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<AzureOpenAISettings>>().Value);

        builder.Services.AddSingleton(sp =>
        {
            var s = sp.GetRequiredService<AzureOpenAISettings>();
            return new AzureOpenAIClient(
                new Uri(s.Endpoint),
                new ApiKeyCredential(s.ApiKey)
            );
        });

        return builder;
    }

}
