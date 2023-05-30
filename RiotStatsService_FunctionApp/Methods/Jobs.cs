using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace RiotStatsService_FunctionApp
{
    class Jobs
    {
        public static void saveMatchData<MatchDataModel>(string savePath, List<MatchDataModel> mDataList, bool append = false) where MatchDataModel : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(mDataList);
                writer = new StreamWriter(savePath + "\\matches.txt", append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static void saveSummonerData<List>(string savePath, List<string> summonerList, bool append = false) where List : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(summonerList);
                writer = new StreamWriter(savePath + "\\summoner.txt", append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static void saveSummonerPuuidData<List>(string savePath, List<string> summonersPuuid, bool append = false) where List : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(summonersPuuid);
                writer = new StreamWriter(savePath + "\\summonersPuuid.txt", append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static List<MatchDataModel> loadMatchData<MatchDataModel>(string filePath) where MatchDataModel : new()
        {
            TextReader reader = null;
            try
            {
                try
                {
                    reader = new StreamReader(filePath + "\\matches.txt");
                    var fileContents = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<MatchDataModel>>(fileContents.ToString());
                }
                catch
                {
                    using (StreamWriter w = File.AppendText(filePath + "\\matches.txt"))
                        return null;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public static List<string> loadSummonerData<List>(string filePath) where List : new()
        {
            TextReader reader = null;
            try
            {
                try
                {
                    reader = new StreamReader(filePath + "\\summoner.txt");
                    var fileContents = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<string>>(fileContents.ToString());
                }
                catch
                {
                    using (StreamWriter w = File.AppendText(filePath + "\\summoner.txt"))
                        return null;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public static List<string> loadSummonerPuuidData<List>(string filePath) where List : new()
        {
            TextReader reader = null;
            try
            {
                try
                {
                    reader = new StreamReader(filePath + "\\summonersPuuid.txt");
                    var fileContents = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<string>>(fileContents.ToString());
                }
                catch
                {
                    using (StreamWriter w = File.AppendText(filePath + "\\summonersPuuid.txt"))
                        return null;
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }


    }
}
