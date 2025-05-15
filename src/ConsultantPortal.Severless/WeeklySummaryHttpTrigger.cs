using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ConsultantPortal.Severless;

public class WeeklySummaryHttpTrigger
{
    private readonly ILogger<WeeklySummaryHttpTrigger> _logger;

    public WeeklySummaryHttpTrigger(ILogger<WeeklySummaryHttpTrigger> logger)
    {
        _logger = logger;
    }

    [Function("WeeklySummaryHttpTrigger")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}