using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using static RiotStatsService_FunctionApp.Function1;

namespace RiotStatsService_FunctionApp
{
    class DiscordController
    {
        public static string URL = "";
        public static void SetDiscordWebHook(ILogger log)
        {
            if (isTest){
                log.LogInformation("Assigning TEST URL as webhook");
                URL = Environment.GetEnvironmentVariable("TestDiscordWebhook");
            }
            else{
                log.LogInformation("Assigning LIVE URL as webhook");
                URL = Environment.GetEnvironmentVariable("DiscordWebhook");
            }
        }
        
        public static void sendDiscWebhookMessage(string message, ILogger log)
        {
            SetDiscordWebHook(log);
            
            HttpClient postWebhook = new HttpClient();

            log.LogInformation("Sending Discord webhook (message) using: " + URL);

            try
            {
                var values = new List<KeyValuePair<string, string>>
                {
                    // add values to data for post
                    new KeyValuePair<string, string>("username", "Stats Bot"),
                    new KeyValuePair<string, string>("content", message),
                };

                FormUrlEncodedContent content = new FormUrlEncodedContent(values);
                var result = postWebhook.PostAsync(URL, content);
            }
            catch (AggregateException ex)
            {
                // get all possible exceptions which are thrown
                log.LogError("Error sending Discord webhook: ", ex.Message);
            }
            log.LogInformation("Successfully sent Discord webhook {message}");
        }

        public static void sendDiscWebhookChart(string chartURL, ILogger log)
        {
            // Dynamically sets the discord web hook location
            SetDiscordWebHook(log);
            HttpClient postWebhook = new HttpClient();

            log.LogInformation("Sending Discord webhook (chart) using: " + URL);

            try
            {
                var values = new List<KeyValuePair<string, string>>
                {
                    // add values to data for post
                    new KeyValuePair<string, string>("username", "Stats Bot"),
                    new KeyValuePair<string, string>("content", chartURL),
                };

                FormUrlEncodedContent content = new FormUrlEncodedContent(values);
                var result = postWebhook.PostAsync(URL, content);
            }
            catch (AggregateException ex)
            {
                // get all possible exceptions which are thrown
                log.LogError("Error sending Discord webhook: ", ex.Message);
            }
            log.LogInformation("Successfully sent Discord webhook {charts}");
        }

        public static string buildDiscMessage(Dictionary<string, KdaTotalsModel> kdaResultsDictionary, List<double> kdaRankingList, Dictionary<string, double> mostKillsIn10Games, Dictionary<string, double> combinedKillsOneGame, Dictionary<string, double> mostKillsAllTime, Dictionary<string, double> mostAssistsAllTime, Dictionary<string, double> mostDeathsAllTime,  ILogger log)
        {
            kdaRankingList = kdaRankingList.OrderBy(x => x).ToList();
            Dictionary<string, KdaTotalsModel> tempDict = new Dictionary<string, KdaTotalsModel>();
           
            log.LogInformation("Building Discord message");
            
            StringBuilder stringBuilder = new StringBuilder("__**This is League Of Legends Stats Bot**__");
            stringBuilder.AppendFormat(Environment.NewLine);
            stringBuilder.AppendLine("__**The following is a rolling 10 match KDA for ARAM - in order of most to least shameful**__");
            stringBuilder.AppendFormat(Environment.NewLine);
            
            foreach (var x in kdaResultsDictionary)
            {
                tempDict.Add(x.Key, x.Value);
            }

            foreach (var kda in kdaRankingList)
            {
                foreach(var summoner in tempDict)
                {
                    if (summoner.Value.averageKDA == kda)
                    {
                        stringBuilder.AppendFormat("Summoner __**{0}**__, has an average KDA of __**{1}**__", summoner.Key, kda);
                        stringBuilder.AppendFormat(Environment.NewLine);
                        tempDict.Remove(summoner.Key);
                    }
                }
            }

            stringBuilder.AppendFormat(Environment.NewLine);
            stringBuilder.AppendLine("__**The following is a rolling 10 match combined kill count - in order of most to least shameful**__");
            stringBuilder.AppendFormat(Environment.NewLine);

            foreach (var item in combinedKillsOneGame)
            {
                stringBuilder.AppendFormat("Summoner __**{0}**__ has a combined total of __**{1}**__ kills in their last 10 games", item.Key, item.Value);
                stringBuilder.AppendFormat(Environment.NewLine);
            }

            stringBuilder.AppendFormat(Environment.NewLine);
            stringBuilder.AppendFormat("The highest kill count (single game) in the last 10 games was by __**{0}**__ with __**{1}**__ kills", mostKillsIn10Games.FirstOrDefault().Key, mostKillsIn10Games.FirstOrDefault().Value);
            stringBuilder.AppendFormat(Environment.NewLine);
            
            stringBuilder.AppendFormat(Environment.NewLine);
            stringBuilder.AppendFormat(Environment.NewLine);
            stringBuilder.AppendFormat("############ **** High Scores **** ############");
            stringBuilder.AppendFormat(Environment.NewLine);
            stringBuilder.AppendFormat(Environment.NewLine);
            stringBuilder.AppendFormat("The highest kill count on record: __**{0}**__ with __**{1}**__ kills", mostKillsAllTime.FirstOrDefault().Key, mostKillsAllTime.FirstOrDefault().Value);
            stringBuilder.AppendFormat(Environment.NewLine);

            stringBuilder.AppendFormat(Environment.NewLine);
            stringBuilder.AppendFormat("The highest assist count on record: __**{0}**__ with __**{1}**__ assists", mostAssistsAllTime.FirstOrDefault().Key, mostAssistsAllTime.FirstOrDefault().Value);
            stringBuilder.AppendFormat(Environment.NewLine);

            stringBuilder.AppendFormat(Environment.NewLine);
            stringBuilder.AppendFormat("The highest death count on record: __**{0}**__ with __**{1}**__ deaths", mostDeathsAllTime.FirstOrDefault().Key, mostDeathsAllTime.FirstOrDefault().Value);
            stringBuilder.AppendFormat(Environment.NewLine);
            
            stringBuilder.AppendFormat(Environment.NewLine);
            stringBuilder.AppendFormat("############ **** High Scores **** ############");

            string content = stringBuilder.ToString();
            log.LogInformation("Successfully built Discord message");
            return content;
        }

        public static void sendDiscMessage(Dictionary<string, KdaTotalsModel> kdaResultsDictionary, List<double> kdaRankingList, Dictionary<string, double> mostKillsIn10Games, Dictionary<string, double> combinedKillsOneGame, Dictionary<string, double> mostKillsAllTime, Dictionary<string, double> mostAssistsAllTime, Dictionary<string, double> mostDeathsAllTime, ILogger log)
        {
            var messageContent = buildDiscMessage(kdaResultsDictionary, kdaRankingList, mostKillsIn10Games, combinedKillsOneGame, mostKillsAllTime, mostAssistsAllTime, mostDeathsAllTime, log);
            sendDiscWebhookMessage(messageContent, log);
        }

        public static void sendDiscMessage(string chartURL, ILogger log)
        {
            sendDiscWebhookChart(chartURL, log);
        }

    }
}
