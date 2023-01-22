using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using static RiotStatsService_FunctionApp.AzureBlobController;
using System.Text.Json;
using System.Collections.Generic;

namespace RiotStatsService_FunctionApp
{
    public class ChartFunction
    {
        public static string chartURL = "";
        public static bool blobChartUrlExists;
        public static bool blobChartDataExists;
        private static bool containerStorageExists;
        
        [FunctionName("ChartFunction")]
        //public void Run([TimerTrigger("* * * * * *")]TimerInfo myTimer, ILogger log)
        public void Run([TimerTrigger("0 30 16 * * Sun")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function ChartFunction executed at: {DateTime.Now}");

            log.LogInformation("Calling sendDiscMessage method override for ChartURL");

            containerStorageExists = AzureBlobController.CheckContainerStorageExists(log);
            blobChartDataExists = CheckBlobExists(log, "chartData.json");
            
            if (containerStorageExists && blobChartDataExists)
            {
                var chartDataBinary = LoadFromStorage(log, "chartData.json");
                var chartData = JsonSerializer.Deserialize<Dictionary<string, List<double>>>(chartDataBinary);
                
                int count = 0;
                foreach (var item in chartData)
                {
                    count = item.Value.Count;
                    break;
                }
                chartURL = ChartBuilder.BuildChart(chartData, count, log);
            }
   
            log.LogInformation("ChartURL: " + chartURL);
            DiscordController.sendDiscMessage(chartURL, log);
        }
    }
}
