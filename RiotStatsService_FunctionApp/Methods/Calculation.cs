using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace RiotStatsService_FunctionApp
{
    public class Calculations
    {

        public static void GetKDA(Dictionary<string, List<MatchDataModel>> matchDataDictionary, List<KdaModel> kdaModelList, ILogger log)
        {
            try
            {
                foreach (var puuidMatchSet in matchDataDictionary)
                {
                    foreach (var match in puuidMatchSet.Value)
                    {
                        foreach (var participant in match.Info.Participants)
                        {
                            log.LogInformation("Getting KDA for {0}", participant.Puuid);
                            if (participant.Puuid == puuidMatchSet.Key)
                            {
                                var kdaModel = new KdaModel();
                                kdaModel.assists = participant.Assists;
                                kdaModel.deaths = participant.Deaths;
                                kdaModel.kills = participant.Kills;
                                kdaModel.name = participant.SummonerName;
                                kdaModel.puuid = participant.Puuid;
                                kdaModelList.Add(kdaModel);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.LogError("Error creating kdaModel from matchDataDictionary: ", e.Message);
            }
        }
        
        public static void Calculate10MatchKDA(List<KdaModel> kdaModelList, string summonerPuuid, Dictionary<string, KdaTotalsModel> kdaResultsDictionary, ILogger log)
        {
            var kdaTotal = new KdaTotalsModel();
            var name = "";

            foreach (var model in kdaModelList)
            {
                if(model.puuid == summonerPuuid)
                {
                    kdaTotal.totalKills += model.kills;
                    kdaTotal.totalAssists += model.assists;
                    kdaTotal.totalDeaths += model.deaths;
                    if (string.IsNullOrWhiteSpace(name)){
                        name = model.name;
                    }
                }
            }
            kdaTotal.averageKDA = Math.Round((kdaTotal.totalKills + kdaTotal.totalAssists) / kdaTotal.totalDeaths, 2);

            try
            {
                log.LogInformation("Adding {0} to kdaResultsDictionary", name);
                kdaResultsDictionary.Add(name, kdaTotal);
            }
            catch (Exception e)
            {
                log.LogError("Error adding {0}, {1} to kdaResultsDictionary: ", name, kdaTotal, e.Message);
            }

        }

        public static void GetMostKillsIn10Games(List<KdaModel> kdaModelList, Dictionary<string, double> mostKillsIn10Games, ILogger log)
        {
            kdaModelList = kdaModelList.OrderBy(x => x.kills).ToList();
            mostKillsIn10Games.Add(kdaModelList.LastOrDefault().name, kdaModelList.LastOrDefault().kills);
        }
        
        public static void GetMostAssistsIn10Games(List<KdaModel> kdaModelList, Dictionary<string, double> mostAssistsIn10Games, ILogger log)
        {
            kdaModelList = kdaModelList.OrderBy(x => x.assists).ToList();
            mostAssistsIn10Games.Add(kdaModelList.LastOrDefault().name, kdaModelList.LastOrDefault().assists);
        }

        public static void GetMostDeathsIn10Games(List<KdaModel> kdaModelList, Dictionary<string, double> mostDeathsIn10Games, ILogger log)
        {
            kdaModelList = kdaModelList.OrderBy(x => x.deaths).ToList();
            mostDeathsIn10Games.Add(kdaModelList.LastOrDefault().name, kdaModelList.LastOrDefault().deaths);
        }

        public static void GetCombinedKillsIn10Games(Dictionary<string, KdaTotalsModel> kdaResultsDictionary, Dictionary<string, double> combinedKillsOneGame, ILogger log)
        {
            var orderedList = new List<double>();
            Dictionary<string, KdaTotalsModel> tempDict = new Dictionary<string, KdaTotalsModel>();
            
            foreach (var x in kdaResultsDictionary)
            {
                tempDict.Add(x.Key, x.Value);
            }
           
            foreach (var t in tempDict)
            {
                orderedList.Add(t.Value.totalKills);
            }
            orderedList = orderedList.OrderBy(x => x).ToList();
            foreach (var t in orderedList)
            {
                foreach (var t2 in tempDict)
                {
                    if (t2.Value.totalKills == t)
                    {
                        log.LogInformation("Adding {0} to combinedKillsOneGame", t2.Key);
                        combinedKillsOneGame.Add(t2.Key, t2.Value.totalKills);
                        tempDict.Remove(t2.Key);
                    }
                }
            }
        }

        public static void CompareMostKills(Dictionary<string, double> mostKillsIn10Games, Dictionary<string, double> mostKillsAllTime, ILogger log)
        {
            log.LogInformation("Calculating highest ever kill count");
            //If the mostKillsAllTime dictionary already has a value, compare the current value to the previous value and add the highest value to the dictionary
            try
            {
                if (mostKillsAllTime.Count > 0)
                {
                    if (mostKillsIn10Games.FirstOrDefault().Value > mostKillsAllTime.FirstOrDefault().Value)
                    {
                        mostKillsAllTime.Clear();
                        log.LogInformation("Updating first mostKillsAllTime dictionary record with new values");
                        mostKillsAllTime.Add(mostKillsIn10Games.FirstOrDefault().Key, mostKillsIn10Games.FirstOrDefault().Value);
                    }
                }
                else
                {
                    //If the mostKillsAllTime dictionary is empty, add the current value to the dictionary
                    log.LogInformation("Populating first mostKillsAllTime dictionary record");
                    mostKillsAllTime.Add(mostKillsIn10Games.FirstOrDefault().Key, mostKillsIn10Games.FirstOrDefault().Value);
                }
            }
            catch (Exception e)
            {
                log.LogError("Error comparing most kills: ", e.Message);
            }

        }

        public static void CompareMostAssists(Dictionary<string, double> mostAssistsIn10Games, Dictionary<string, double> mostAssistsAllTime, ILogger log)
        {
            log.LogInformation("Calculating highest ever assist count");
            //If the mostKillsAllTime dictionary already has a value, compare the current value to the previous value and add the highest value to the dictionary
            try
            {
                if (mostAssistsAllTime.Count > 0)
                {
                    if (mostAssistsIn10Games.FirstOrDefault().Value > mostAssistsAllTime.FirstOrDefault().Value)
                    {
                        mostAssistsAllTime.Clear();
                        log.LogInformation("Updating first mostAssistsAllTime dictionary record with new values");
                        mostAssistsAllTime.Add(mostAssistsIn10Games.FirstOrDefault().Key, mostAssistsIn10Games.FirstOrDefault().Value);
                    }
                }
                else
                {
                    //If the mostKillsAllTime dictionary is empty, add the current value to the dictionary
                    log.LogInformation("Populating first mostAssistsAllTime dictionary record");
                    mostAssistsAllTime.Add(mostAssistsIn10Games.FirstOrDefault().Key, mostAssistsIn10Games.FirstOrDefault().Value);
                }
            }
            catch (Exception e)
            {
                log.LogError("Error comparing most assists: ", e.Message);
            }

        }

        public static void CompareMostDeaths(Dictionary<string, double> mostDeathsIn10Games, Dictionary<string, double> mostDeathsAllTime, ILogger log)
        {
            log.LogInformation("Calculating highest ever death count");
            //If the mostKillsAllTime dictionary already has a value, compare the current value to the previous value and add the highest value to the dictionary
            try
            {
                if (mostDeathsAllTime.Count > 0)
                {
                    if (mostDeathsIn10Games.FirstOrDefault().Value > mostDeathsAllTime.FirstOrDefault().Value)
                    {
                        mostDeathsAllTime.Clear();
                        log.LogInformation("Updating first mostdeathsAllTime dictionary record with new values");
                        mostDeathsAllTime.Add(mostDeathsIn10Games.FirstOrDefault().Key, mostDeathsIn10Games.FirstOrDefault().Value);
                    }
                }
                else
                {
                    //If the mostKillsAllTime dictionary is empty, add the current value to the dictionary
                    log.LogInformation("Populating first mostdeathsAllTime dictionary record");
                    mostDeathsAllTime.Add(mostDeathsIn10Games.FirstOrDefault().Key, mostDeathsIn10Games.FirstOrDefault().Value);
                }
            }
            catch (Exception e)
            {
                log.LogError("Error comparing most deaths: ", e.Message);
            }

        }

        public static int winsInLast10Games(List<MatchDataModel> matchdata, List<string> summonerPuuidList)
        {
            var winCount = 0;
            foreach (var match in matchdata)
            {
                if (match.Metadata != null)
                {
                    foreach (var participant in match.Info.Participants)
                    {
                        if (participant.Puuid == summonerPuuidList[0] && participant.Win == true)
                        {
                            winCount++;
                        }
                    }
                }
            }
            return winCount;
        }

        public static void last10Games(List<MatchDataModel> matchdata, List<string> summonerPuuidList)
        {
            Console.WriteLine("** LAST 10 GAMES PLAYED **\n");
            foreach (var match in matchdata)
            {
                if (match.Metadata != null)
                {
                    foreach (var participant in match.Info.Participants)
                    {
                        if (participant.Puuid == summonerPuuidList[0])
                        {
                            Console.WriteLine("Champion: {0}\nKills: {1}\nDeaths: {2}\n", participant.ChampionName, participant.Kills, participant.Deaths);
                        }
                    }
                }
            }
        }

        public static List<MatchOverviewModel> calculateOverviewStats(List<MatchDataModel> matchdata, List<string> summonerPuuidList)
        {
            IDictionary<string, List<MatchOverviewModel>> matchOverviewDictionary = new Dictionary<string, List<MatchOverviewModel>>();

            foreach (var match in matchdata)
            {
                List<MatchOverviewModel> masterGameList = new List<MatchOverviewModel>();
                List<Participant> team1 = new List<Participant>();
                List<Participant> team2 = new List<Participant>();
                MatchOverviewModel team1Overview = new MatchOverviewModel();
                MatchOverviewModel team2Overview = new MatchOverviewModel();

                if (match.Metadata != null)
                {
                    //var gameDuration = TimeConversion.SecondsToMinutes(match.Info.GameDuration);
                    //Console.WriteLine("gameDuration: " + gameDuration);

                    // sort the 10 unordered participants into two teams
                    foreach (var participant in match.Info.Participants)
                    {
                        sortTeams(participant, team1, team2);
                    }

                    calculateTeamStats(match, team1Overview, team2Overview, team1, team2, summonerPuuidList, masterGameList, matchOverviewDictionary);
                    calculatePlayerStatsForMatch(team1, team2);

                    matchOverviewDictionary.Add(match.Metadata.MatchId, masterGameList);
                }
            }

            foreach (var matchOverview in matchOverviewDictionary)
            {
                foreach (var value in matchOverview.Value)
                {

                }
            }

            return null;
        }

        public static void calculateTeamStats(MatchDataModel match, MatchOverviewModel team1Overview, MatchOverviewModel team2Overview, List<Participant> team1, List<Participant> team2, List<string> summonerPuuidList, List<MatchOverviewModel> masterGameList, IDictionary<string, List<MatchOverviewModel>> matchOverviewDictionary)
        {
            //calculate stats for team 1. save to a MatchOverviewModel which holds team level information
            team1Overview.teamID = teamCheck(team1);
            team1Overview.matchID = match.Metadata.MatchId;
            team1Overview.totalKills = calculateTotalTeamKills(team1);
            team1Overview.totalAssists = calculateTotalTeamAssists(team1);
            team1Overview.totalDeaths = calculateTotalTeamDeaths(team1);
            team1Overview.totalHealing = calculateTotalTeamHealing(team1);
            team1Overview.totalGoldEarned = calculateTotalTeamGold(team1);
            team1Overview.totalGoldSpent = calculateTotalTeamGoldSpent(team1);
            team1Overview.winGame = winCheck(team1);
            team1Overview.myTeam = summonerCheck(team1, summonerPuuidList);

            //calculate stats for team 2. save to a MatchOverviewModel which holds team level information
            team2Overview.teamID = teamCheck(team2);
            team2Overview.matchID = match.Metadata.MatchId;
            team2Overview.totalKills = calculateTotalTeamKills(team2);
            team2Overview.totalAssists = calculateTotalTeamAssists(team2);
            team2Overview.totalDeaths = calculateTotalTeamDeaths(team2);
            team2Overview.totalHealing = calculateTotalTeamHealing(team2);
            team2Overview.totalGoldEarned = calculateTotalTeamGold(team2);
            team2Overview.totalGoldSpent = calculateTotalTeamGoldSpent(team2);
            team2Overview.winGame = winCheck(team2);
            team2Overview.myTeam = summonerCheck(team2, summonerPuuidList);

            //adds both MatchOverviewModel to a masterList
            masterGameList.AddMany(team1Overview, team2Overview);
        }

        public static void calculatePlayerStatsForMatch(List<Participant> team1, List<Participant> team2)
        {

        }

        public static bool summonerCheck(List<Participant> team, List<string> summonerPuuidList)
        {
            foreach (var player in team)
            {
                if (player.Puuid.Contains(summonerPuuidList[0]))
                {
                    return true;
                }
            }
            return false;
        }

        public static int teamCheck(List<Participant> team)
        {
            if (team[0].TeamId == 100) { return 1; }
            else { return 2; }
        }

        public static bool winCheck(List<Participant> team)
        {
            if (team[0].Win == true) { return true; }
            else { return false; }
        }

        public static int calculateTotalTeamKills(List<Participant> team)
        {
            var totalTeamKills = 0;
            foreach (var player in team)
            {
                totalTeamKills = totalTeamKills + player.Kills;
            }
            return totalTeamKills;
        }

        public static int calculateTotalTeamDeaths(List<Participant> team)
        {
            var totalTeamDeaths = 0;
            foreach (var player in team)
            {
                totalTeamDeaths = totalTeamDeaths + player.Deaths;
            }
            return totalTeamDeaths;
        }

        public static int calculateTotalTeamAssists(List<Participant> team)
        {
            var totalTeamAssists = 0;
            foreach (var player in team)
            {
                totalTeamAssists = totalTeamAssists + player.Assists;
            }
            return totalTeamAssists;
        }

        public static int calculateTotalTeamHealing(List<Participant> team)
        {
            var totalTeamHealing = 0;
            foreach (var player in team)
            {
                totalTeamHealing = totalTeamHealing + player.TotalHealsOnTeammates;
            }
            return totalTeamHealing;
        }

        public static int calculateTotalTeamGold(List<Participant> team)
        {
            var totalTeamGold = 0;
            foreach (var player in team)
            {
                totalTeamGold = totalTeamGold + player.GoldEarned;
            }
            return totalTeamGold;
        }

        public static int calculateTotalTeamGoldSpent(List<Participant> team)
        {
            var totalTeamGoldSpent = 0;
            foreach (var player in team)
            {
                totalTeamGoldSpent = totalTeamGoldSpent + player.GoldSpent;
            }
            return totalTeamGoldSpent;
        }

        public static void sortTeams(Participant participant, List<Participant> team1, List<Participant> team2)
        {
            if (participant.TeamId == 100)
            {
                team1.Add(participant);
            }
            else if (participant.TeamId == 200)
            {
                team2.Add(participant);
            }
        }   
    }
}
