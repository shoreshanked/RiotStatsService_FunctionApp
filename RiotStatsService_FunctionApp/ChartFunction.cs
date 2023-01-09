using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using static RiotStatsService_FunctionApp.Function1;

namespace RiotStatsService_FunctionApp
{
    public class ChartFunction
    {
        [FunctionName("ChartFunction")]
        public void Run([TimerTrigger("0 30 16 * * MON")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function ChartFunction executed at: {DateTime.Now}");

            log.LogInformation("Calling sendDiscMessage method override for ChartURL");
            log.LogInformation("ChartURL: " + chartURL);
            DiscordController.sendDiscMessage(chartURL, log);
        }
    }
}
