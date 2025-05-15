using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;


namespace ConsultantPortal.Api.AzureFunctions;
public class WeeklySummaryFunction
{

    private readonly ILogger _logger;
    public WeeklySummaryFunction(
        ILoggerFactory loggerFactory)
    {

        _logger = loggerFactory.CreateLogger<WeeklySummaryFunction>();
    }

}