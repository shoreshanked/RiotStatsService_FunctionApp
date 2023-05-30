using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiotStatsService_FunctionApp
{
    public partial class Highscores
    {
        public MostKillsAllTime mostKillsAllTime { get; set; }
        public MostAssistsAllTime mostAssistsAllTime { get; set; }
        public MostDeathsAllTime mostDeathsAllTime { get; set; }
    }
    public class MostAssistsAllTime
    {
        public string Summoner { get; set; }
        public double Count { get; set; }
    }

    public class MostDeathsAllTime
    {
        public string Summoner { get; set; }
        public double Count { get; set; }
    }

    public class MostKillsAllTime
    {
        public string Summoner { get; set; }
        public double Count { get; set; }
    }

}
