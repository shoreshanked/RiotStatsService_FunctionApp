using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace RiotStatsService_FunctionApp
{ 
    public class Function1
    {
        RestClient client = new RestClient("https://euw1.api.riotgames.com");
        RestClient clientRegion = new RestClient("https://europe.api.riotgames.com");
        string apiToken = Environment.GetEnvironmentVariable("RiotGamesAPIKey");
        string allTimeStatsSet = Environment.GetEnvironmentVariable("AllTimeStatsSet");
        string storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        
        public static bool isTest = true;

        Dictionary<string, List<string>> matchIdDictionary = new Dictionary<string, List<string>>();
        Dictionary<string, List<MatchDataModel>> matchDataDictionary = new Dictionary<string, List<MatchDataModel>>();
        Dictionary<string, KdaTotalsModel> kdaResultsDictionary = new Dictionary<string, KdaTotalsModel>();
        Dictionary<string, SummonerModel> summonerResponseDict = new Dictionary<string, SummonerModel>();

        Dictionary<string, double> mostKillsIn10Games = new Dictionary<string, double>();
        Dictionary<string, double> mostAssistsIn10Games = new Dictionary<string, double>();
        Dictionary<string, double> mostDeathsIn10Games = new Dictionary<string, double>();

        Dictionary<string, double> mostKillsAllTime = new Dictionary<string, double>();
        Dictionary<string, double> mostAssistsAllTime = new Dictionary<string, double>();
        Dictionary<string, double> mostDeathsAllTime = new Dictionary<string, double>();
        
        Dictionary<string, double> combinedKillsOneGame = new Dictionary<string, double>();

        //Variables for chart building
        Dictionary<string, List<double>> chartData = new Dictionary<string, List<double>>();
        public static string chartURL = "";

        //List Variables
        List<string> summonerList = new List<string>()
        {
            "Rick n Two Crows",
            "The Master Queef",
            "Up the Ashe",
            "The Meshsiah",
            "The Rum Ham",
            "Ninjahobo"
        };
        
        List<string> summonerPuuidList = new List<string>();
        List<KdaModel> kdaModelList = new List<KdaModel>();
        List<string> matchIdList = new List<string>();
        List<double> kdaRankingList = new List<double>();

        
        public void setDict(ILogger log)
        {
            mostKillsAllTime.Add("Up the Ashe", 36.0);
            mostAssistsAllTime.Add("The Master Queef", 51.0);
            mostDeathsAllTime.Add("Up the Ashe", 15.0);
            log.LogInformation("Initialising all time stats on first run\n");
        }

        [FunctionName("Function1")]
        //public void Run([TimerTrigger("* * * * * *")]TimerInfo myTimer, ILogger log) // Dev
        public void Run([TimerTrigger("0 0 16 * * *")] TimerInfo myTimer, ILogger log) //Live
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
    
            if (allTimeStatsSet == "false")
            {
                setDict(log);
                Environment.SetEnvironmentVariable("AllTimeStatsSet", "true");
                log.LogInformation("Updating AllTimeStatsSet environment variable to true\n");
            }
                
            //Summoner info passed back to a dictionary for use later
            summonerResponseDict = SummonerController.getSummonerPuuid(summonerList, apiToken, client, log);

            foreach(var summonerResponse in summonerResponseDict)
            {
                summonerPuuidList.Add(summonerResponse.Value.puuid);
            }

            if (summonerPuuidList.Count > 0)
            {
                MatchController.getSummonerMatches(summonerPuuidList, apiToken, client, matchIdDictionary, matchIdList, log);
            }
            if(matchIdDictionary.Count > 0)
            {
                MatchController.getMatchData(matchIdDictionary, apiToken, clientRegion, matchDataDictionary, log);
            }

            //Loops through the matchData and then adds a KdaModel per player to the kdaModelList
            log.LogInformation("Calling GetKDA Method");
            Calculations.GetKDA(matchDataDictionary, kdaModelList, log);

            //For each summoner in the summoner list, check the corresponding kdaModel and then calculate thier 10 match rolling kda
            foreach(var summonerPuuid in summonerPuuidList)
            {
                Thread.Sleep(1000);
                log.LogInformation("Calling Calculate10MatchKDA Method for {0}", summonerPuuid);
                Calculations.Calculate10MatchKDA(kdaModelList, summonerPuuid, kdaResultsDictionary, log);
            }

            //Create a list of the averageKDA scores - this is to be ordered later as a dictionary cannot be ordered
            foreach (var summoner in kdaResultsDictionary)
            {
                log.LogInformation("Adding KDA to kdaRankingList for {0}", summoner.Key);
                kdaRankingList.Add((double)summoner.Value.averageKDA);
            }

            //Orders the kdaModelList by the kill count and adds the resulting top kill count to the mostKills dictionary
            log.LogInformation("Calling GetMostKillsIn10Games Method");
            Calculations.GetMostKillsIn10Games(kdaModelList, mostKillsIn10Games, log);
            log.LogInformation("Calling GetMostAssistsIn10Games Method");
            Calculations.GetMostAssistsIn10Games(kdaModelList, mostAssistsIn10Games, log);
            log.LogInformation("Calling GetMostDeathsIn10Games Method");
            Calculations.GetMostDeathsIn10Games(kdaModelList, mostDeathsIn10Games, log);

            log.LogInformation("Calling GetCombinedKillsIn10Games Method");
            Calculations.GetCombinedKillsIn10Games(kdaResultsDictionary, combinedKillsOneGame, log);

            //Building dicionaries for the most kills, assists and deaths ever - these are never cleared down 
            log.LogInformation("Calling CompareMostKills Method");
            Calculations.CompareMostKills(mostKillsIn10Games, mostKillsAllTime, log);
            log.LogInformation("Calling CompareMostAssists Method");
            Calculations.CompareMostAssists(mostAssistsIn10Games, mostAssistsAllTime, log);
            log.LogInformation("Calling CompareMostDeaths Method");
            Calculations.CompareMostDeaths(mostDeathsIn10Games, mostDeathsAllTime, log);

            
            log.LogInformation("Calling InitChartData Method");
            chartURL = ChartBuilder.InitChartData(chartData, kdaResultsDictionary, log);
            log.LogInformation("Chart URL: {0}", chartURL);

            //testing chart builder - superceded by azure function and needs to be commented out when live
            //DiscordController.sendDiscMessage(chartURL, log);

            log.LogInformation("Calling sendDiscMessage Method");
            DiscordController.sendDiscMessage(kdaResultsDictionary, kdaRankingList, mostKillsIn10Games, combinedKillsOneGame, mostKillsAllTime, mostAssistsAllTime, mostDeathsAllTime, log);

            //Clear all objects to prevent memory leaks
            log.LogInformation("Clearing all objects");
            resetObjects();  
        }

        public void resetObjects()
        {
            matchIdDictionary.Clear();
            matchDataDictionary.Clear();
            kdaResultsDictionary.Clear();
            summonerResponseDict.Clear();
            summonerList.Clear();
            summonerPuuidList.Clear();
            kdaModelList.Clear();
            matchIdList.Clear();
            kdaRankingList.Clear();
            mostKillsIn10Games.Clear();
            mostAssistsIn10Games.Clear();
            mostDeathsIn10Games.Clear();
            combinedKillsOneGame.Clear();
        }
    }
    public static class ListExtenstions
    {
        public static void AddMany<T>(this List<T> list, params T[] elements)
        {
            list.AddRange(elements);
        }
    } 
}
