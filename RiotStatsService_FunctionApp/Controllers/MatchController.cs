using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Threading;
using System.Threading.Tasks;

namespace RiotStatsService_FunctionApp
{
    class MatchController
    {
        static public void getSummonerMatches(List<String> summonerPuuidList, string apiToken, RestClient clientRegion, Dictionary<string, List<string>> matchIdDictionary, List<string> matchIdList, ILogger log)
        {
            foreach (var puuid in summonerPuuidList)
            {
                var getMatchesViaPuuid = new RestRequest("https://europe.api.riotgames.com/lol/match/v5/matches/by-puuid/" + puuid + "/ids?queue=450&start=0&count=10", Method.Get);
                getMatchesViaPuuid.AddHeader("X-Riot-Token", apiToken);

                log.LogInformation("Getting match details from API for {0}", puuid);
                try
                {
                    var response = clientRegion.GetAsync(getMatchesViaPuuid).Result;
                    matchIdList = response.Content.Replace("\"", "").Trim('[', ']').Split(',').ToList();
                    matchIdDictionary.Add(puuid, matchIdList);
                }
                catch (Exception e)
                {
                    log.LogError("Error getting match details from API: ", e.Message);
                }
                log.LogInformation("Successfully retrieved match details from API");
            }
        }

        static public void getMatchData(Dictionary<string, List<string>> matchIdDictionary, string apiToken, RestClient clientRegion, Dictionary<string, List<MatchDataModel>> matchDataDictionary, ILogger log)
        {
            foreach (var puuid in matchIdDictionary)
            {
                var matchDataList = new List<MatchDataModel>();
                MatchDataModel data = new MatchDataModel();

                foreach (var matchID in puuid.Value)
                {
                    var getMatchDataViaMatchID = new RestRequest("https://europe.api.riotgames.com/lol/match/v5/matches/" + matchID, Method.Get);
                    getMatchDataViaMatchID.AddHeader("X-Riot-Token", apiToken);

                    log.LogInformation("Getting data for match: {0} from API for {1}", matchID, puuid.Key);
                    try
                    {
                        Thread.Sleep(1000);
                        var matchdata = clientRegion.ExecuteGetAsync(getMatchDataViaMatchID).Result;
                        data = JsonConvert.DeserializeObject<MatchDataModel>(matchdata.Content);
                        matchDataList.Add(data);
                    }
                    catch (Exception e)
                    {
                        log.LogError("Error getting match data from API: ", e.Message);

                    }   
                }
                log.LogInformation("Successfully retrieved match data from API");

                try
                {
                    log.LogInformation("Adding puuid.Key & matchDataList to matchDataDictionary");
                    matchDataDictionary.Add(puuid.Key, matchDataList);
                }
                catch (Exception e)
                {
                    log.LogError("Error adding match data to matchDataDictionary: ", e.Message);
                }
            }
        }
    }
}
