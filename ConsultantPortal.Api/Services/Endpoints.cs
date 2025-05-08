using Azure.AI.OpenAI;
using ConsultantPortal.Api.Models;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;

namespace ConsultantPortal.Api.Services;

public static class Endpoints
{
    public static void MapTimeLogEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/timelogs");
        group.MapGet("/", async (ICosmosDbService<TimeLog> db) => await db.GetItemsAsync("SELECT * FROM c"));
        group.MapGet("/{id}", async (string id, ICosmosDbService<TimeLog> db) => await db.GetItemAsync(id) is TimeLog tl ? Results.Ok(tl) : Results.NotFound());
        group.MapPost("/", async (TimeLog tl, ICosmosDbService<TimeLog> db) => Results.Created($"/timelogs{(await db.CreateItemAsync(tl)).Id}", tl));
        group.MapPut("/{id}",async (string id, TimeLog tl, ICosmosDbService<TimeLog> db)=>await db.UpdateItemAsync(id, tl));
        group.MapDelete("/{id}",async(string id, ICosmosDbService<TimeLog> db)=>await db.DeleteItemAsync(id)?Results.NoContent():Results.NotFound());
    }
    public static void MapProjectEndpoints(this WebApplication app) {
        var group = app.MapGroup("/api/projects");

        // GET all
        group.MapGet("/", async (ICosmosDbService<Project> db) =>
            await db.GetItemsAsync("SELECT * FROM c"));

        // GET by id
        group.MapGet("/{id}", async (string id, ICosmosDbService<Project> db) =>
            await db.GetItemAsync(id) is Project proj
                ? Results.Ok(proj)
                : Results.NotFound());

        // POST (create)
        group.MapPost("/", async (Project proj, ICosmosDbService<Project> db) =>
        {
            var created = await db.CreateItemAsync(proj);
            return Results.Created($"/projects/{created.Id}", created);
        });

        // PUT (update)
        group.MapPut("/{id}", async (string id, Project proj, ICosmosDbService<Project> db) =>
            await db.UpdateItemAsync(id, proj) is Project updated
                ? Results.Ok(updated)
                : Results.NotFound());

        // DELETE
        group.MapDelete("/{id}", async (string id, ICosmosDbService<Project> db) =>
            await db.DeleteItemAsync(id)
                ? Results.NoContent()
                : Results.NotFound());

    }
    public static void MapClientEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/clients");

        // GET all
        group.MapGet("/", async (ICosmosDbService<Client> db) =>
            await db.GetItemsAsync("SELECT * FROM c"));

        // GET by id
        group.MapGet("/{id}", async (string id, ICosmosDbService<Client> db) =>
            await db.GetItemAsync(id) is Client client
                ? Results.Ok(client)
                : Results.NotFound());

        // POST (create)
        group.MapPost("/", async (Client client, ICosmosDbService<Client> db) =>
        {
            var created = await db.CreateItemAsync(client);
            return Results.Created($"/clients/{created.Id}", created);
        });

        // PUT (update)
        group.MapPut("/{id}", async (string id, Client client, ICosmosDbService<Client> db) =>
            await db.UpdateItemAsync(id, client) is Client updated
                ? Results.Ok(updated)
                : Results.NotFound());

        // DELETE
        group.MapDelete("/{id}", async (string id, ICosmosDbService<Client> db) =>
            await db.DeleteItemAsync(id)
                ? Results.NoContent()
                : Results.NotFound());

    }
    public static void MapGenerateSummaryEndpoint(this WebApplication app)
    {
        app.MapPost("/api/generate-summary", async (
        [FromBody] IEnumerable<TimeLog> logs,
        AzureOpenAIClient client,
        ICosmosDbService<Summary> db) =>
        {
            // 1️. Build the chat-message sequence
            var messages = new List<ChatMessage>
                    {
                        new SystemChatMessage(
                            "You are a helpful assistant that summarizes time logs into concise overviews."
                        ),
                        new UserChatMessage(
                            "Summarize these time logs:\n" +
                            string.Join('\n',
                                logs.Select(l =>
                                    $"{l.Date:yyyy-MM-dd}: {l.Hours}h – {l.Description}"
                                )
                            )
                        )
                    };

            // 2️. Get the chat client and send the messages
            var chatClient = client.GetChatClient("gpt-35-turbo");
            var response = await chatClient.CompleteChatAsync(
                messages,
                new ChatCompletionOptions
                {
                    Temperature = 0.7f,
                    FrequencyPenalty = 0f,
                    PresencePenalty = 0f,
                }
            );

            // 3️. Pull out the last message’s text as your summary
            var summary = response.Value
                                  .Content
                                  .Last()
                                  .Text
                                  .Trim();

            // 4️. (Optional) Persist to Cosmos DB
            var record = new Summary
            {
                Id = Guid.NewGuid().ToString(),
                Content = summary,
                Created = DateTime.UtcNow
            };
            await db.CreateItemAsync(record);

            // 5️. Return a 200 OK with the JSON payload
            return Results.Ok(new { summary });
        });
    }
}
