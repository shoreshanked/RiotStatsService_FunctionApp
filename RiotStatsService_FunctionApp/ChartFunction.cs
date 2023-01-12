using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using static RiotStatsService_FunctionApp.Function1;
using static RiotStatsService_FunctionApp.AzureBlobController;

namespace RiotStatsService_FunctionApp
{
    public class ChartFunction
    {
        public static string chartURL = "";
        public static bool blobChartUrlExists;
        private static bool containerStorageExists;
        
        [FunctionName("ChartFunction")]
        public void Run([TimerTrigger("0 30 16 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function ChartFunction executed at: {DateTime.Now}");

            log.LogInformation("Calling sendDiscMessage method override for ChartURL");

            containerStorageExists = AzureBlobController.CheckContainerStorageExists(log);
            blobChartUrlExists = CheckBlobExists(log, "chartURL.txt");
            
            if (containerStorageExists && blobChartUrlExists)
            {
                var chartURLBinary = LoadFromStorage(log, "chartURL.txt");
                chartURL = chartURLBinary.ToString();
            }
            
            log.LogInformation("ChartURL: " + chartURL);
            DiscordController.sendDiscMessage(chartURL, log);
        }
    }
}
