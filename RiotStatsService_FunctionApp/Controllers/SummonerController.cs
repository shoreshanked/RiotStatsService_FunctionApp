using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace RiotStatsService_FunctionApp
{
    class SummonerController
    {

        static public Dictionary<string, SummonerModel> getSummonerPuuid(List<String> summonerList, string apiToken, RestClient client, ILogger log)
        {
            SummonerModel response = new SummonerModel();
            Dictionary<string, SummonerModel> summonerResponseDict = new Dictionary<string, SummonerModel>();

            
            foreach (var summoner in summonerList)
            {
                log.LogInformation("Getting summoner details from API for {0}", summoner);
                var getSummoner = new RestRequest("/lol/summoner/v4/summoners/by-name/" + summoner, Method.Get);
                getSummoner.AddHeader("X-Riot-Token", apiToken);

                try
                {
                    response = client.GetAsync<SummonerModel>(getSummoner).Result;
                    summonerResponseDict.Add(summoner, response);
                }
                catch (Exception e)
                {
                    log.LogError("Error getting summoner details from API: ", e.Message);
                }
            }
            log.LogInformation("Successfully retrieved summoner details from API");
            return summonerResponseDict;
        }

    }
}
